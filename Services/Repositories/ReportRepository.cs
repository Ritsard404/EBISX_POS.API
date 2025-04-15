﻿using EBISX_POS.API.Data;
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

        public async Task<XInvoiceReportDTO> XInvoiceReport()
        {
            var pesoCulture = new CultureInfo("en-PH");
            var defaultDecimal = 0m;
            var defaultDate = new DateTime(2000, 1, 1);

            // Safely fetch data with null checks
            var orders = await _dataContext.Order
                .Include(o => o.Cashier)
                .Include(o => o.Items)
                .Include(o => o.AlternativePayments)
                    .ThenInclude(ap => ap.SaleType)
                .Where(o => !o.IsRead)
                .ToListAsync() ?? new List<Order>();

            var posInfo = await _dataContext.PosTerminalInfo.FirstOrDefaultAsync();
            if (posInfo == null)
            {
                throw new InvalidOperationException("POS terminal information not configured");
            }

            var ts = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Include(t => t.ManagerLog)
                .OrderBy(t => t.Id)
                .LastOrDefaultAsync();

            // Handle empty orders scenario
            var firstOrder = orders.FirstOrDefault();
            var lastOrder = orders.LastOrDefault();

            // Calculate financials with null protection
            decimal openingFundDec = ts?.CashInDrawerAmount ?? defaultDecimal;
            decimal withdrawnAmount = await _dataContext.ManagerLog
                .Where(mw => mw.Timestamp == ts)
                .SumAsync(mw => mw.WithdrawAmount);

            decimal voidDec = orders.Where(o => o.IsCancelled)
                                  .Sum(o => o?.TotalAmount ?? defaultDecimal);
            decimal refundDec = orders.Where(o => o.IsReturned)
                                    .Sum(o => o?.TotalAmount ?? defaultDecimal);

            decimal validOrdersTotal = orders.Where(o => !o.IsCancelled && !o.IsReturned)
                                           .Sum(o => o?.TotalAmount ?? defaultDecimal);

            decimal shortOverDec = openingFundDec + validOrdersTotal
                                 - ((ts?.CashOutDrawerAmount ?? defaultDecimal) - withdrawnAmount);

            // Safe payment processing
            var payments = new Payments
            {
                Cash = orders.Sum(o => o?.CashTendered ?? defaultDecimal),
                OtherPayments = orders
                    .SelectMany(o => o.AlternativePayments ?? new List<AlternativePayments>())
                    .GroupBy(ap => ap.SaleType?.Name ?? "Unknown")
                    .Select(g => new PaymentDetail
                    {
                        Name = g.Key,
                        Amount = g.Sum(x => x?.Amount ?? defaultDecimal)
                    }).ToList()
            };

            var summary = new TransactionSummary
            {
                CashInDrawer = ((ts?.CashOutDrawerAmount ?? defaultDecimal) - withdrawnAmount)
                              .ToString("C", pesoCulture),
                OtherPayments = payments.OtherPayments
            };

            // Build DTO with safe values
            var dto = new XInvoiceReportDTO
            {
                BusinessName = posInfo.RegisteredName ?? "N/A",
                OperatorName = posInfo.OperatedBy ?? "N/A",
                AddressLine = posInfo.Address ?? "N/A",
                VatRegTin = posInfo.VatTinNumber ?? "N/A",
                Min = posInfo.MinNumber ?? "N/A",
                SerialNumber = posInfo.PosSerialNumber ?? "N/A",

                ReportDate = DateTime.Now.ToString("MMMM dd, yyyy", pesoCulture),
                ReportTime = DateTime.Now.ToString("hh:mm tt", pesoCulture),
                StartDateTime = firstOrder?.CreatedAt.LocalDateTime.ToString("MM/dd/yy  hh:mm tt", pesoCulture)
                              ?? defaultDate.ToString("MM/dd/yy  hh:mm tt", pesoCulture),
                EndDateTime = lastOrder?.CreatedAt.LocalDateTime.ToString("MM/dd/yy  hh:mm tt", pesoCulture)
                             ?? defaultDate.ToString("MM/dd/yy  hh:mm tt", pesoCulture),

                Cashier = ts?.Cashier != null
                        ? $"{ts.Cashier.UserFName} {ts.Cashier.UserLName}"
                        : "N/A",
                BeginningOrNumber = firstOrder?.Id.ToString("D12") ?? "N/A",
                EndingOrNumber = lastOrder?.Id.ToString("D12") ?? "N/A",

                OpeningFund = openingFundDec.ToString("C", pesoCulture),
                VoidAmount = voidDec.ToString("C", pesoCulture),
                Refund = refundDec.ToString("C", pesoCulture),
                Withdrawal = withdrawnAmount.ToString("C", pesoCulture),

                Payments = payments,
                TransactionSummary = summary,
                ShortOver = shortOverDec.ToString("C", pesoCulture)
            };

            // Mark orders as read if any exist
            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    order.IsRead = true;
                }
                await _dataContext.SaveChangesAsync();
            }

            return dto;
        }

        public async Task<ZInvoiceReportDTO> ZInvoiceReport()
        {
            var pesoCulture = new CultureInfo("en-PH");
            var defaultDecimal = 0m;
            var defaultDate = new DateTime(2000, 1, 1);
            var today = DateTime.Today;


            // Initialize empty collections to prevent null references
            var allTimestamps = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Include(t => t.ManagerLog)
                .ToListAsync() ?? new List<Timestamp>();

            var orders = await _dataContext.Order
                .Include(o => o.Items)
                .Include(o => o.AlternativePayments)
                    .ThenInclude(ap => ap.SaleType)
                .ToListAsync() ?? new List<Order>();

            var posInfo = await _dataContext.PosTerminalInfo.FirstOrDefaultAsync() ?? new PosTerminalInfo
            {
                RegisteredName = "N/A",
                OperatedBy = "N/A",
                Address = "N/A",
                VatTinNumber = "N/A",
                MinNumber = "N/A",
                PosSerialNumber = "N/A",
                AccreditationNumber = "N/A",
                DateIssued = DateTime.Now,
                PtuNumber = "N/A",
                ValidUntil = DateTime.Now
            };

            // Handle empty scenario for dates
            var startDate = orders.Any() ? orders.Min(t => t.CreatedAt.LocalDateTime) : defaultDate;
            var endDate = orders.Any() ? orders.Max(t => t.CreatedAt.LocalDateTime) : defaultDate;

            // Calculate values with null protection
            var withdrawnAmount = allTimestamps
                .SelectMany(t => t.ManagerLog)
                .Where(mw => mw?.Action == "Withdrawal")
                .Sum(mw => mw?.WithdrawAmount ?? defaultDecimal);

            var regularOrders = orders.Where(o => !o.IsCancelled && !o.IsReturned).ToList();
            var voidOrders = orders.Where(o => o.IsCancelled).ToList();
            var returnOrders = orders.Where(o => o.IsReturned).ToList();

            // Accumulated Sales
            decimal previousAccumulatedSales = regularOrders.Where(c => c.CreatedAt.Date < today).Sum(o => o?.TotalAmount ?? defaultDecimal);
            decimal salesForTheDay = regularOrders.Where(c => c.CreatedAt.Date == today).Sum(o => o?.TotalAmount ?? defaultDecimal);
            decimal presentAccumulatedSales = previousAccumulatedSales + salesForTheDay;

            // Financial calculations with default values
            decimal grossSales = regularOrders.Sum(o => o?.TotalAmount ?? defaultDecimal);
            decimal totalVoid = voidOrders.Sum(o => o?.TotalAmount ?? defaultDecimal);
            decimal totalReturns = returnOrders.Sum(o => o?.TotalAmount ?? defaultDecimal);
            decimal totalDiscounts = regularOrders.Sum(o => o?.DiscountAmount ?? defaultDecimal);
            decimal cashSales = regularOrders.Sum(o => o?.CashTendered ?? defaultDecimal);
            decimal netAmount = grossSales - totalReturns - totalVoid - totalDiscounts;

            // VAT calculations with defaults
            decimal vatableSales = regularOrders.Sum(v => v?.VatSales ?? defaultDecimal);
            decimal vatAmount = regularOrders.Sum(o => o?.VatAmount ?? defaultDecimal);
            decimal vatExempt = regularOrders.Sum(o => o?.VatExempt ?? defaultDecimal);
            decimal zeroRated = 0m;

            decimal cashInDrawer = allTimestamps.Sum(s => s.CashOutDrawerAmount) ?? defaultDecimal;
            decimal openingFund = allTimestamps.LastOrDefault()?.CashInDrawerAmount ?? defaultDecimal;

            decimal expectedCash = openingFund + cashSales;
            decimal actualCash = cashInDrawer + withdrawnAmount;
            decimal shortOver = actualCash - expectedCash;

            var knownDiscountTypes = Enum.GetNames(typeof(DiscountTypeEnum)).ToList();

            decimal seniorDiscount = regularOrders
                .Where(s => s.DiscountType == DiscountTypeEnum.Senior.ToString())
                .Sum(s => s.DiscountAmount) ?? 0m;

            decimal pwdDiscount = regularOrders
                .Where(s => s.DiscountType == DiscountTypeEnum.Pwd.ToString())
                .Sum(s => s.DiscountAmount) ?? 0m;

            decimal otherDiscount = regularOrders
                .Where(s => !knownDiscountTypes.Contains(s.DiscountType))
                .Sum(s => s.DiscountAmount) ?? 0m;

            // Safe payment processing
            var payments = new Payments
            {
                Cash = cashSales,
                OtherPayments = orders
                .SelectMany(o => o.AlternativePayments != null ? o.AlternativePayments : new List<AlternativePayments>())
                .GroupBy(ap => ap.SaleType?.Name ?? "Unknown")
                .Select(g => new PaymentDetail
                {
                    Name = g.Key,
                    Amount = g.Sum(x => x.Amount)
                }).ToList()
            };

            // Build DTO with zero defaults
            var dto = new ZInvoiceReportDTO
            {
                BusinessName = posInfo.RegisteredName ?? "N/A",
                OperatorName = posInfo.OperatedBy ?? "N/A",
                AddressLine = posInfo.Address ?? "N/A",
                VatRegTin = posInfo.VatTinNumber ?? "N/A",
                Min = posInfo.MinNumber ?? "N/A",
                SerialNumber = posInfo.PosSerialNumber ?? "N/A",

                ReportDate = DateTime.Now.ToString("MMMM dd, yyyy", pesoCulture),
                ReportTime = DateTime.Now.ToString("hh:mm tt", pesoCulture),
                StartDateTime = startDate.ToString("MM/dd/yy  hh:mm tt", pesoCulture),
                EndDateTime = endDate.ToString("MM/dd/yy  hh:mm tt", pesoCulture),

                // Order numbers
                BeginningSI = GetOrderNumber(regularOrders.Min(o => o?.Id)),
                EndingSI = GetOrderNumber(regularOrders.Max(o => o?.Id)),
                BeginningVoid = GetOrderNumber(voidOrders.Min(o => o?.Id)),
                EndingVoid = GetOrderNumber(voidOrders.Max(o => o?.Id)),
                BeginningReturn = GetOrderNumber(returnOrders.Min(o => o?.Id)),
                EndingReturn = GetOrderNumber(returnOrders.Max(o => o?.Id)),

                // Always zero when empty
                ResetCounter = posInfo.ResetCounterNo.ToString(),
                ZCounter = posInfo.ZCounterNo.ToString(),

                // Financial summaries
                PresentAccumulatedSales = presentAccumulatedSales.ToString("C", pesoCulture),
                PreviousAccumulatedSales = previousAccumulatedSales.ToString("C", pesoCulture),
                SalesForTheDay = salesForTheDay.ToString("C", pesoCulture),

                SalesBreakdown = new SalesBreakdown
                {
                    VatableSales = vatableSales.ToString("C", pesoCulture),
                    VatAmount = vatAmount.ToString("C", pesoCulture),
                    VatExemptSales = vatExempt.ToString("C", pesoCulture),
                    ZeroRatedSales = zeroRated.ToString("C", pesoCulture),
                    GrossAmount = grossSales.ToString("C", pesoCulture),
                    LessDiscount = totalDiscounts.ToString("C", pesoCulture),
                    LessReturn = totalReturns.ToString("C", pesoCulture),
                    LessVoid = totalVoid.ToString("C", pesoCulture),
                    LessVatAdjustment = defaultDecimal.ToString("C", pesoCulture),
                    NetAmount = netAmount.ToString("C", pesoCulture)
                },

                TransactionSummary = new TransactionSummary
                {
                    CashInDrawer = cashSales.ToString("C", pesoCulture),
                    OtherPayments = payments.OtherPayments
                },

                DiscountSummary = new DiscountSummary
                {
                    SeniorCitizen = seniorDiscount.ToString("C", pesoCulture),
                    PWD = pwdDiscount.ToString("C", pesoCulture),
                    Other = otherDiscount.ToString("C", pesoCulture)
                },

                SalesAdjustment = new SalesAdjustment
                {
                    Return = totalReturns.ToString("C", pesoCulture),
                    Void = totalVoid.ToString("C", pesoCulture),
                },

                VatAdjustment = new VatAdjustment
                {
                    SCTrans = defaultDecimal.ToString("C", pesoCulture),
                    PWDTrans = defaultDecimal.ToString("C", pesoCulture),
                    RegDiscTrans = defaultDecimal.ToString("C", pesoCulture),
                    ZeroRatedTrans = defaultDecimal.ToString("C", pesoCulture),
                    VatOnReturn = defaultDecimal.ToString("C", pesoCulture),
                    OtherAdjustments = defaultDecimal.ToString("C", pesoCulture)
                },

                OpeningFund = openingFund.ToString("C", pesoCulture),
                Withdrawal = withdrawnAmount.ToString("C", pesoCulture),
                PaymentsReceived = (cashSales + payments.OtherPayments.Sum(s => s.Amount)).ToString("C", pesoCulture),
                ShortOver = shortOver.ToString("C", pesoCulture)
            };

            posInfo.ZCounterNo += 1;
            await _dataContext.SaveChangesAsync();

            return dto;
        }

        private string GetOrderNumber(long? orderId)
        {
            return orderId.HasValue ? orderId.Value.ToString("D12") : 0.ToString("D12");
        }
    }
}
