using EBISX_POS.API.Data;
using EBISX_POS.API.Models;
using EBISX_POS.API.Models.Utils;
using EBISX_POS.API.Services.DTO.Order;
using EBISX_POS.API.Services.DTO.Report;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EBISX_POS.API.Services.Repositories
{
    public class ReportRepository(DataContext _dataContext) : IReport
    {
        public async Task<(string CashInDrawer, string CurrentCashDrawer)> CashTrack(string cashierEmail)
        {
            var timestamp = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Where(t => t.Cashier.UserEmail == cashierEmail && t.TsOut == null && t.CashInDrawerAmount != null && t.CashInDrawerAmount >= 1000)
                .FirstOrDefaultAsync();

            if (timestamp == null || timestamp.CashInDrawerAmount == null)
                return ("₱0.00", "₱0.00");

            var tsIn = timestamp.TsIn;

            decimal totalCashInDrawer = await _dataContext.Order
                    .Where(o =>
                        o.Cashier.UserEmail == cashierEmail &&
                        !o.IsCancelled &&
                        !o.IsPending &&
                        !o.IsReturned &&
                        o.CreatedAt >= tsIn &&
                        o.CashTendered != null &&
                        o.TotalAmount != 0
                    )
                    .SumAsync(o =>
                        o.CashTendered!.Value > o.TotalAmount
                            ? o.TotalAmount
                            : o.CashTendered!.Value
                    );

            var phCulture = new CultureInfo("en-PH");
            string cashInDrawerText = timestamp.CashInDrawerAmount.Value.ToString("C", phCulture);
            string currentCashDrawerText = (timestamp.CashInDrawerAmount.Value + totalCashInDrawer).ToString("C", phCulture);

            return (cashInDrawerText, currentCashDrawerText);
        }

        public async Task<List<GetInvoicesDTO>> GetInvoicesByDate(DateTime dateTime)
        {
            return await _dataContext.Order
                .Include(o => o.Cashier)
                .Where(d => d.CreatedAt.DateTime == dateTime && !d.IsCancelled && !d.IsPending && !d.IsReturned)
                .Select(s => new GetInvoicesDTO()
                {
                    InvoiceNum = s.Id,
                    DateTime = s.CreatedAt.ToString("MM/dd/yyyy hh:mm tt"),
                    CashierName = s.Cashier.UserFName + " " + s.Cashier.UserLName,
                    CashierEmail = s.Cashier.UserEmail,
                })
                .ToListAsync();
        }

        public async Task<List<GetInvoiceDTO>> GetInvoiceById(long invId)
        {
            // 1) Load the order, its cashier, items and alternative payments
            var order = await _dataContext.Order
                .Include(o => o.Cashier)
                .Include(o => o.Items)
                .Include(o => o.AlternativePayments)
                    .ThenInclude(ap => ap.SaleType)
                .FirstOrDefaultAsync(o => o.Id == invId);

            if (order == null)
                return new List<GetInvoiceDTO>();

            var orderItems = await GetOrderItems(order.Id);

            // 2) Load your POS terminal / business info (assumes a single row)
            var posInfo = await _dataContext.Set<PosTerminalInfo>()
                .FirstOrDefaultAsync();

            // 3) Map to your DTO
            var dto = new GetInvoiceDTO
            {
                // --- Business Details from POS info
                RegisteredName = posInfo?.RegisteredName ?? "",
                Address = posInfo?.Address ?? "",
                VatTinNumber = posInfo?.VatTinNumber ?? "",
                MinNumber = posInfo?.MinNumber ?? "",

                // --- Invoice Header
                InvoiceNum = order.Id.ToString(),
                InvoiceDate = order.CreatedAt
                                          .ToString("MM/dd/yyyy HH:mm:ss"),
                OrderType = order.OrderType,
                CashierName = $"{order.Cashier.UserFName} {order.Cashier.UserLName}",

                // --- Line Items

                Items = orderItems
                .Select(group => new ItemDTO
                {
                    // take the quantity of the first (parent) sub‐order
                    Qty = group.TotalQuantity,

                    // map every sub‐order into your ItemInfoDTO
                    itemInfos = group.SubOrders?
                        .Select(s => new ItemInfoDTO
                        {
                            Description = s.DisplayName,
                            Amount = s.ItemPriceString
                        })
                        .ToList()
                        // ensure non‐null list
                        ?? new List<ItemInfoDTO>()
                })
                .ToList(),

                // --- Totals
                TotalAmount = (order.TotalAmount).ToString("C2"),
                DiscountAmount = (order.DiscountAmount ?? 0m).ToString("C2"),
                DueAmount = (order.DueAmount ?? 0m).ToString("C2"),
                CashTenderAmount = (order.CashTendered ?? 0m).ToString("C2"),
                TotalTenderAmount = (order.TotalTendered ?? 0m).ToString("C2"),
                ChangeAmount = (order.ChangeAmount ?? 0m).ToString("C2"),

                // VAT breakdown
                VatExemptSales = (order.VatExempt ?? 0m).ToString("C2"),
                VatSales = ((order.TotalAmount - (order.VatExempt ?? 0m)))
                                      .ToString("C2"),
                VatAmount = (order.VatAmount ?? 0m).ToString("C2"),

                // Other tenders (e.g. gift cert, card, etc.)
                OtherPayments = order.AlternativePayments
                    .Select(ap => new OtherPaymentDTO
                    {
                        SaleTypeName = ap.SaleType.Name,
                        Amount = ap.Amount.ToString("C2")
                    })
                    .ToList(),

                // PWD/Senior/etc.
                ElligiblePeopleDiscounts = order.EligiblePwdScNames?
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList()
                ?? new List<string>(),

                // --- POS Details
                PosSerialNumber = posInfo?.PosSerialNumber ?? "",
                DateIssued = posInfo?.DateIssued.ToString("MM/dd/yyyy") ?? "",
                ValidUntil = posInfo?.ValidUntil.ToString("MM/dd/yyyy") ?? ""
            };

            return new List<GetInvoiceDTO> { dto };
        }

        private async Task<List<GetCurrentOrderItemsDTO>> GetOrderItems(long orderId)
        {
            var items = await _dataContext.Order
                .Include(o => o.Items)
                .Include(c => c.Coupon)
                .Where(o => o.Id == orderId)
                .SelectMany(o => o.Items)
                .Where(i => !i.IsVoid)
                .Include(i => i.Menu)
                .Include(i => i.Drink)
                .Include(i => i.AddOn)
                .Include(i => i.Order)
                .Include(i => i.Meal)
                .ToListAsync();

            // Group items by EntryId.
            // For items with no EntryId (child meals), use the parent's EntryId from the Meal property.
            var groupedItems = items
                .GroupBy(i => i.EntryId ?? i.Meal?.EntryId)
                .OrderBy(g => g.Min(i => i.createdAt))
                .Select(g =>
                {
                    // Compute the promo discount amount from the parent order.
                    var promoDiscount = g.Select(i => (i.Order?.DiscountType == DiscountTypeEnum.Promo.ToString()
                                                        ? i.Order?.DiscountAmount ?? 0m
                                                        : 0m))
                                         .FirstOrDefault();
                    // Check for other discount types.
                    var otherDiscount = g.Any(i => i.IsPwdDiscounted || i.IsSeniorDiscounted);

                    // Set HasDiscount to true if there's any other discount or promo discount value is greater than zero.
                    var hasDiscount = otherDiscount || (promoDiscount > 0m);

                    // Build the DTO from the group
                    var dto = new GetCurrentOrderItemsDTO
                    {
                        // Use the group's key or 0 if still null.
                        EntryId = g.Key ?? "",
                        HasDiscount = hasDiscount,
                        PromoDiscountAmount = promoDiscount,
                        IsPwdDiscounted = g.Any(i => i.IsPwdDiscounted),
                        IsSeniorDiscounted = g.Any(i => i.IsSeniorDiscounted),
                        // Order each group so that the parent (Meal == null) comes first.
                        SubOrders = g.OrderBy(i => i.Meal == null ? 0 : 1)
                                     .Select(i => new CurrentOrderItemsSubOrder
                                     {
                                         MenuId = i.Menu?.Id,
                                         DrinkId = i.Drink?.Id,
                                         AddOnId = i.AddOn?.Id,
                                         // Fallback: use Menu name first, then Drink, then AddOn.
                                         Name = i.Menu?.MenuName ?? i.Drink?.MenuName ?? i.AddOn?.MenuName ?? "Unknown",
                                         Size = i.Menu?.Size ?? i.Drink?.Size ?? i.AddOn?.Size,
                                         ItemPrice = i.ItemPrice ?? 0m,
                                         Quantity = i.ItemQTY ?? 1,
                                         IsFirstItem = i.Meal == null
                                     })
                                     .ToList()
                    };

                    // If discount applies, add an extra suborder for discount details.
                    if (dto.HasDiscount && dto.PromoDiscountAmount <= 0)
                    {
                        // Calculate discount based on the current total of suborders.
                        // (Be aware that if you add the discount as a suborder, it might affect TotalPrice.)
                        var discountAmount = dto.SubOrders.Sum(s => s.ItemSubTotal) >= 250
                        ? 250
                        : dto.SubOrders.Sum(s => s.ItemSubTotal) * 0.20m;

                        // Use the first item in the group to determine discount type.
                        var discountName = g.Any(i => i.IsPwdDiscounted) ? "PWD" : "Senior";

                        dto.SubOrders.Add(new CurrentOrderItemsSubOrder
                        {
                            Name = discountName,          // This can be adjusted to show a more descriptive name.
                            ItemPrice = discountAmount, // The discount amount.
                            Quantity = 1,
                            // You can set Size to null or leave it empty.
                            IsFirstItem = false         // Typically discount line is not the first item.
                        });
                    }

                    return dto;
                })
                .ToList();

            var ordersWithCoupons = await _dataContext.Order
                .Include(o => o.Coupon)
                .ThenInclude(c => c.CouponMenus)
                .Where(o => o.Id == orderId)
                .ToListAsync();

            var couponItems = ordersWithCoupons
                .SelectMany(o => o.Coupon)
                .Where(c => c != null)
                .DistinctBy(c => c.CouponCode) // using DistinctBy from System.Linq if available
                .Select(c => new GetCurrentOrderItemsDTO
                {
                    EntryId = $"Coupon-{c.CouponCode}",
                    HasDiscount = false,
                    PromoDiscountAmount = 0m,
                    IsPwdDiscounted = false,
                    IsSeniorDiscounted = false,
                    CouponCode = c.CouponCode,
                    SubOrders = new List<CurrentOrderItemsSubOrder>
                    {
                        new CurrentOrderItemsSubOrder
                        {
                            Name = $"Coupon: {c.CouponCode}",
                            ItemPrice = c.PromoAmount ?? 0m,
                            Quantity = c.CouponItemQuantity ?? 0,
                            IsFirstItem = true
                        }
                    }
                    .Concat(
                        c.CouponMenus?.Where(m => m.MenuIsAvailable)
                        .Select(m => new CurrentOrderItemsSubOrder
                        {
                            MenuId = m.Id,
                            Name = m.MenuName,
                            Size = m.Size,
                            ItemPrice = 0m,
                            Quantity = 1,
                            IsFirstItem = false
                        }) ?? Enumerable.Empty<CurrentOrderItemsSubOrder>()
                    ).ToList()
                })
                .ToList();



            // ✅ Merge regular orders with coupon orders.
            groupedItems.AddRange(couponItems);

            return groupedItems;
        }

    }
}
