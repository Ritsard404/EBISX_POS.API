using EBISX_POS.API.Data;
using EBISX_POS.API.Models;
using EBISX_POS.API.Models.Utils;
using EBISX_POS.API.Services.DTO.Order;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EBISX_POS.API.Services.Repositories
{
    public class OrderRepository(DataContext _dataContext) : IOrder
    {
        public async Task<(bool, string)> AddCurrentOrderVoid(AddCurrentOrderVoidDTO voidOrder)
        {
            // ✅ Collect valid menu IDs (avoiding unnecessary IDs)
            var menuIds = new List<int?> { voidOrder.menuId, voidOrder.drinkId, voidOrder.addOnId }
                .Where(id => id > 0)
                .Cast<int>()
                .ToList();

            // ✅ Fetch relevant menu items in a single query
            var menuItems = await _dataContext.Menu
                .Where(m => menuIds.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id);

            // ✅ Retrieve selected items (null if not found)
            var menu = menuItems.GetValueOrDefault(voidOrder.menuId ?? 0);
            var drink = menuItems.GetValueOrDefault(voidOrder.drinkId ?? 0);
            var addOn = menuItems.GetValueOrDefault(voidOrder.addOnId ?? 0);

            // ✅ Validation: At least one valid item must be selected
            if (menu == null && drink == null && addOn == null)
            {
                return (false, "At least one item (Menu, Drink, or AddOn) must be selected.");
            }

            // ✅ Check for pending order with the active cashier
            var currentOrder = await _dataContext.Order
                .Include(o => o.Items)
                .Include(o => o.Cashier)
                .FirstOrDefaultAsync(o =>
                    o.IsPending &&
                    o.Cashier != null &&
                    o.Cashier.UserEmail == voidOrder.cashierEmail &&
                    o.Cashier.IsActive
                );

            // ✅ Create a new order if none exists
            if (currentOrder == null)
            {
                var users = await _dataContext.User
                    .Where(u => (u.UserEmail == voidOrder.cashierEmail || u.UserEmail == voidOrder.managerEmail) && u.IsActive)
                    .ToDictionaryAsync(u => u.UserEmail);

                if (!users.ContainsKey(voidOrder.cashierEmail)) return (false, "Cashier not found.");
                if (!users.ContainsKey(voidOrder.managerEmail)) return (false, "Unauthorized Card!");

                currentOrder = new Order
                {
                    OrderType = "Cancelled",
                    TotalAmount = 0m,
                    CreatedAt = DateTimeOffset.Now,
                    Cashier = users[voidOrder.cashierEmail],
                    IsPending = false,
                    IsCancelled = true,
                    Items = new List<Item>()
                };

                await _dataContext.Order.AddAsync(currentOrder);
            }

            // ✅ Track total amount of voided items
            decimal totalAmount = 0m;

            // ✅ Add voided items efficiently
            void AddVoidItem(Menu? item, decimal? customPrice = null, Item? parentMeal = null, bool isDrink = false, bool isAddOn = false)
            {
                if (item == null) return;

                var voidedItem = new Item
                {
                    ItemQTY = voidOrder.qty,
                    ItemPrice = customPrice ?? item.MenuPrice,
                    Menu = !isDrink && !isAddOn && parentMeal == null ? item : null,
                    Drink = isDrink ? item : null,
                    AddOn = isAddOn ? item : null,
                    Meal = parentMeal,
                    Order = currentOrder,
                    IsVoid = true,
                    VoidedAt = DateTimeOffset.Now
                };

                currentOrder.Items.Add(voidedItem);
                totalAmount += (voidedItem.ItemPrice ?? 0m) * (voidedItem.ItemQTY ?? 1);
            }

            // ✅ Void Menu (can have drink/addOn linked)
            var mealItem = menu != null ? new Item
            {
                ItemQTY = voidOrder.qty,
                ItemPrice = menu.MenuPrice,
                Menu = menu,
                Order = currentOrder,
                IsVoid = true,
                VoidedAt = DateTimeOffset.Now
            } : null;

            if (mealItem != null)
            {
                currentOrder.Items.Add(mealItem);
                totalAmount += (mealItem.ItemPrice ?? 0m) * (mealItem.ItemQTY ?? 1);
            }

            // ✅ Void Drink and AddOn (linked to meal if applicable)
            AddVoidItem(drink, voidOrder.drinkPrice > 0 ? voidOrder.drinkPrice : drink?.MenuPrice, mealItem, isDrink: true);
            AddVoidItem(addOn, voidOrder.addOnPrice > 0 ? voidOrder.addOnPrice : addOn?.MenuPrice, mealItem, isAddOn: true);

            // ✅ Update total order amount
            //currentOrder.TotalAmount += totalAmount;

            // ✅ Save changes to database
            await _dataContext.SaveChangesAsync();

            return (true, "Voided items added successfully.");
        }

        public async Task<(bool, string)> AddOrderItem(AddOrderDTO addOrder)
        {
            var menuIds = new List<int?> { addOrder.menuId, addOrder.drinkId, addOrder.addOnId }
                .Where(id => id > 0)
                .Cast<int>()
                .ToList();

            var menuItems = await _dataContext.Menu
                .Where(m => menuIds.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id);

            var menu = menuItems.GetValueOrDefault(addOrder.menuId ?? 0);
            var drink = menuItems.GetValueOrDefault(addOrder.drinkId ?? 0);
            var addOn = menuItems.GetValueOrDefault(addOrder.addOnId ?? 0);

            if (menu == null && drink == null && addOn == null)
            {
                return (false, "At least one item (Menu, Drink, or AddOn) must be selected.");
            }

            var currentOrder = await _dataContext.Order
                .Include(o => o.Cashier)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.IsPending
                    && o.Cashier != null
                    && o.Cashier.UserEmail == addOrder.cashierEmail
                    && o.Cashier.IsActive);

            if (currentOrder == null)
            {
                var cashier = await _dataContext.User
                    .FirstOrDefaultAsync(u => u.UserEmail == addOrder.cashierEmail && u.IsActive);

                if (cashier == null)
                {
                    return (false, "Cashier not found.");
                }

                currentOrder = new Order
                {
                    OrderType = "",
                    TotalAmount = 0m,
                    CreatedAt = DateTimeOffset.Now,
                    Cashier = cashier,
                    IsPending = true,
                    IsCancelled = false,
                    Items = new List<Item>()
                };

                await _dataContext.Order.AddAsync(currentOrder);
            }

            // ✅ Track total amount of add items
            decimal totalAmount = 0m;

            // ✅ Add add items efficiently
            void AddVoidItem(Menu? item, decimal? customPrice = null, Item? parentMeal = null, bool isDrink = false, bool isAddOn = false)
            {
                if (item == null) return;

                var addItem = new Item
                {
                    ItemQTY = addOrder.qty,
                    EntryId = parentMeal == null ? addOrder.entryId : null,
                    ItemPrice = customPrice ?? item.MenuPrice,
                    Menu = !isDrink && !isAddOn && parentMeal == null ? item : null,
                    Drink = isDrink ? item : null,
                    AddOn = isAddOn ? item : null,
                    Meal = parentMeal,
                    Order = currentOrder,
                };

                currentOrder.Items.Add(addItem);
                totalAmount += (addItem.ItemPrice ?? 0m) * (addItem.ItemQTY ?? 1);
            }

            // Add Menu (can have drink/addOn linked)
            var mealItem = menu != null ? new Item
            {
                ItemQTY = addOrder.qty,
                EntryId = addOrder.entryId,
                ItemPrice = menu.MenuPrice,
                Menu = menu,
                Order = currentOrder,
            } : null;

            if (mealItem != null)
            {
                currentOrder.Items.Add(mealItem);
                totalAmount += (mealItem.ItemPrice ?? 0m) * (mealItem.ItemQTY ?? 1);
            }

            // Add Drink and AddOn (linked to meal if applicable)
            AddVoidItem(drink, addOrder.drinkPrice, mealItem, isDrink: true);
            AddVoidItem(addOn, addOrder.addOnPrice, mealItem, isAddOn: true);

            // Update the order's TotalAmount with the computed total
            currentOrder.TotalAmount += totalAmount;

            await _dataContext.SaveChangesAsync();

            return (true, "Order item added.");
        }

        public async Task<(bool, string)> AddPwdScDiscount(AddPwdScDiscountDTO addPwdScDiscount)
        {
            if (addPwdScDiscount == null)
            {
                return (false, "Invalid discount data.");
            }

            var manager = await _dataContext.User
                .FirstOrDefaultAsync(u => u.UserEmail == addPwdScDiscount.ManagerEmail && u.IsActive);
            var cashier = await _dataContext.User
               .FirstOrDefaultAsync(u => u.UserEmail == addPwdScDiscount.CashierEmail && u.IsActive);

            if (cashier == null || manager == null)
            {
                return (false, "Invalid Credential.");
            }

            // Retrieve current orders for the given cashier.
            var currentOrders = await GetCurrentOrderItems(cashier.UserEmail);

            // Filter orders whose EntryId is in the provided list.
            var ordersToDiscount = currentOrders
                .Where(o => o.EntryId != null && addPwdScDiscount.EntryId.Contains(o.EntryId))
                .ToList();

            if (!ordersToDiscount.Any())
            {
                return (false, "No matching orders found for discount.");
            }

            // Sum all discount amounts from orders.
            var totalDiscountedSubtotal = ordersToDiscount.Sum(o => o.DiscountAmount);

            // Get all orders containing items with matching EntryId(s).
            var orderEntities = await _dataContext.Order
                .Include(o => o.Items)
                .Where(o => o.Items.Any(i => i.EntryId != null && addPwdScDiscount.EntryId.Contains(i.EntryId)) && o.IsPending)
                .ToListAsync();

            var currentOrder = await _dataContext.Order
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.IsPending &&
                                          o.Cashier != null &&
                                          o.Cashier.UserEmail == addPwdScDiscount.CashierEmail &&
                                          o.Cashier.IsActive);

            if (currentOrder == null)
                return (false, "No pending order found for the specified cashier.");
            if (!string.IsNullOrEmpty(currentOrder.DiscountType))
                return (false, "A discount has already been applied to this order.");
            if (currentOrder.Promo != null)
                return (false, "A promotional discount has already been applied to this order. Cannot combine with PWD/Senior discounts.");
            if (currentOrder.Coupon != null && currentOrder.Coupon.Any())
                return (false, "A coupon discount has already been applied to this order. Cannot combine with PWD/Senior discounts.");


            currentOrder.DiscountType = addPwdScDiscount.IsSeniorDisc
                    ? DiscountTypeEnum.Senior.ToString()
                    : DiscountTypeEnum.Pwd.ToString();
            currentOrder.DiscountAmount = totalDiscountedSubtotal;
            currentOrder.EligiblePwdScCount = addPwdScDiscount.PwdScCount;
            currentOrder.OSCAIdsNum = addPwdScDiscount.OSCAIdsNum;
            currentOrder.EligiblePwdScNames = addPwdScDiscount.EligiblePwdScNames;

            foreach (var orderEntity in orderEntities)
            {

                // Update only the items that match the provided EntryId.
                foreach (var item in orderEntity.Items)
                {
                    if (item.EntryId != null && addPwdScDiscount.EntryId.Contains(item.EntryId))
                    {
                        item.IsPwdDiscounted = !addPwdScDiscount.IsSeniorDisc;
                        item.IsSeniorDiscounted = addPwdScDiscount.IsSeniorDisc;
                    }

                    // Update related sub-meals.
                    var subMeal = await _dataContext.Item
                        .Where(i => i.Meal != null && i.Meal.Id == item.Id)
                        .ToListAsync();

                    foreach (var meal in subMeal)
                    {
                        meal.IsPwdDiscounted = !addPwdScDiscount.IsSeniorDisc;
                        meal.IsSeniorDiscounted = addPwdScDiscount.IsSeniorDisc;
                    }
                }
            }

            // Save changes to the database.
            await _dataContext.SaveChangesAsync();

            return (true, "Discount applied successfully.");
        }

        public async Task<(bool, string)> CancelCurrentOrder(string cashierEmail, string managerEmail)
        {
            // Fetch the cashier and manager sequentially to avoid concurrency issues
            var cashier = await _dataContext.User
                .FirstOrDefaultAsync(u => u.UserEmail == cashierEmail && u.IsActive);

            var manager = await _dataContext.User
                .FirstOrDefaultAsync(u => u.UserEmail == managerEmail && u.IsActive);

            // Validate credentials
            if (cashier == null || manager == null)
            {
                return (false, "Invalid Credential.");
            }

            // Retrieve the current pending order for the cashier
            var currentOrder = await _dataContext.Order
                .Include(o => o.Cashier)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.IsPending &&
                                          o.Cashier != null &&
                                          o.Cashier.UserEmail == cashierEmail &&
                                          o.Cashier.IsActive);

            if (currentOrder == null)
            {
                return (false, "No Order Pending");
            }

            // Cancel the order
            currentOrder.IsPending = false;
            currentOrder.IsCancelled = true;

            // Void all items in the order
            var orderItems = await _dataContext.Item
                .Where(i => i.Order.Id == currentOrder.Id)
                .ToListAsync();

            foreach (var item in orderItems)
            {
                item.IsVoid = true;
                item.VoidedAt = DateTimeOffset.Now;
            }

            // Persist changes to the database
            await _dataContext.SaveChangesAsync();

            return (true, "Order Cancelled.");
        }

        public async Task<(bool, string)> AvailCoupon(string cashierEmail, string managerEmail, string couponCode)
        {
            var now = DateTimeOffset.UtcNow;

            // Fetch cashier and manager in a single query
            var users = await _dataContext.User
                .Where(u => (u.UserEmail == cashierEmail || u.UserEmail == managerEmail) && u.IsActive)
                .ToListAsync();

            var cashier = users.FirstOrDefault(u => u.UserEmail == cashierEmail);
            var manager = users.FirstOrDefault(u => u.UserEmail == managerEmail);

            if (cashier == null)
                return (false, "Cashier not found.");
            if (manager == null)
                return (false, "Manager not found.");

            var availedCoupon = await _dataContext.CouponPromo
                .FirstOrDefaultAsync(c => c.CouponCode == couponCode && c.IsAvailable
                                       && (c.ExpirationTime == null || c.ExpirationTime > now));
            if (availedCoupon == null)
                return (false, "Invalid/Expired Coupon Code.");

            // Fetch the current pending order along with its items
            var currentOrder = await _dataContext.Order
                .Include(o => o.Items)
                .Include(o => o.Coupon)
                .FirstOrDefaultAsync(o => o.IsPending);

            if (currentOrder != null && currentOrder.Coupon.Any(c => c.CouponCode == couponCode))
                return (false, "This coupon has already been applied.");

            if (currentOrder == null)
            {

                currentOrder = new Order
                {
                    OrderType = "",
                    TotalAmount = 0m,
                    CreatedAt = DateTimeOffset.Now,
                    Cashier = cashier,
                    IsPending = true,
                    IsCancelled = false,
                    Items = new List<Item>(),
                    Coupon = new List<CouponPromo> { availedCoupon }
                };

                await _dataContext.Order.AddAsync(currentOrder);
            }

            if (currentOrder.Promo != null || currentOrder.EligiblePwdScCount != null)
                return (false, "A discount has already been applied to this order. You cannot apply another discount.");

            if (currentOrder.Coupon != null && currentOrder.Coupon.Count() >= 3)
                return (false, "Coupon limit reached. You can apply a maximum of 3 coupons per order.");

            currentOrder.Coupon.Add(availedCoupon);
            currentOrder.TotalAmount += availedCoupon.PromoAmount ?? 0m;
            currentOrder.DiscountType = DiscountTypeEnum.Coupon.ToString();

            availedCoupon.IsAvailable = false;
            availedCoupon.UpdatedAt = now;

            await _dataContext.SaveChangesAsync();

            return (true, "Coupon Added.");
        }

        public async Task<(bool, string)> EditQtyOrderItem(EditOrderItemQuantityDTO editOrder)
        {
            // Check if the cashier is valid and active
            var cashier = await _dataContext.User
                .FirstOrDefaultAsync(u => u.UserEmail == editOrder.cashierEmail && u.IsActive);

            if (cashier == null)
            {
                return (false, "Cashier not found.");
            }

            // Fetch the item with the associated order and meal (if any)
            var item = await _dataContext.Item
                .Include(i => i.Order)
                    .ThenInclude(o => o.Items)
                .Include(i => i.Meal)
                .FirstOrDefaultAsync(i =>
                    i.EntryId == editOrder.entryId &&
                    i.Order.Cashier.UserEmail == editOrder.cashierEmail &&
                    i.Order.IsPending &&
                    !i.IsVoid
                );

            // Return if item is not found or cannot be modified
            if (item == null)
            {
                return (false, "Item not found or cannot be modified.");
            }

            // Validate quantity (must be greater than 0)
            if (editOrder.qty <= 0)
            {
                return (false, "Quantity must be greater than zero.");
            }

            // Update the main item's quantity
            item.ItemQTY = editOrder.qty;

            // If the item is a parent (i.e. not a child item, indicated by Meal being null),
            // update the quantities for its child items as well.
            if (item.Meal == null)
            {
                var childItems = await _dataContext.Item
                    .Where(i => i.Meal != null && i.Meal.Id == item.Id && !i.IsVoid)
                    .ToListAsync();

                foreach (var childItem in childItems)
                {
                    childItem.ItemQTY = editOrder.qty;
                }
            }

            // Recalculate the order's total amount from scratch:
            var order = item.Order;
            order.TotalAmount = order.Items
                .Where(i => !i.IsVoid)
                .Sum(i => (i.ItemPrice ?? 0m) * (i.ItemQTY ?? 1));

            await _dataContext.SaveChangesAsync();

            return (true, $"Quantity updated to {editOrder.qty}.");
        }

        public async Task<(bool, string)> FinalizeOrder(FinalizeOrderDTO finalizeOrder)
        {
            // Check if the cashier is valid and active
            var cashier = await _dataContext.User
                .FirstOrDefaultAsync(u => u.UserEmail == finalizeOrder.CashierEmail && u.IsActive);

            if (cashier == null)
            {
                return (false, "Cashier not found.");
            }

            var finishOrder = await _dataContext.Order
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.IsPending);

            if (finishOrder == null)
            {
                return (false, "No pending order found.");
            }

            finishOrder.IsPending = false;
            finishOrder.TotalAmount = finalizeOrder.TotalAmount;
            finishOrder.CashTendered = finalizeOrder.CashTendered;
            finishOrder.OrderType = finalizeOrder.OrderType;
            finishOrder.DiscountAmount = finalizeOrder.DiscountAmount;

            await _dataContext.SaveChangesAsync();

            return (true, "Order finished!");
        }

        public async Task<List<GetCurrentOrderItemsDTO>> GetCurrentOrderItems(string? cashierEmail)
        {
            var cashier = await _dataContext.Order
                .Include(o => o.Cashier)
                .Where(s => s.IsPending)
                .Select(c => c.Cashier)
                .FirstOrDefaultAsync();

            if (cashierEmail != null)
            {
                cashier = await _dataContext.User
                    .FirstOrDefaultAsync(u => u.UserEmail == cashierEmail && u.IsActive);
            }

            // If no cashier is found, return an empty list.
            if (cashier == null)
            {
                return new List<GetCurrentOrderItemsDTO>();
            }

            // Fetch all non-voided items from pending orders for the cashier,
            // including related entities needed for the DTO.
            var items = await _dataContext.Order
                .Include(o => o.Items)
                .Include(c => c.Coupon)
                .Where(o => o.IsPending &&
                            o.Cashier != null &&
                            o.Cashier.UserEmail == cashierEmail &&
                            o.Cashier.IsActive)
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
                .Where(o => o.IsPending &&
                            o.Cashier != null &&
                            o.Cashier.UserEmail == cashierEmail &&
                            o.Cashier.IsActive &&
                            o.Coupon.Any())
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

        public async Task<(bool, string)> PromoDiscount(string cashierEmail, string managerEmail, string promoCode)
        {
            var now = DateTimeOffset.UtcNow;

            // Fetch cashier and manager in a single query
            var users = await _dataContext.User
                .Where(u => (u.UserEmail == cashierEmail || u.UserEmail == managerEmail) && u.IsActive)
                .ToListAsync();

            var cashier = users.FirstOrDefault(u => u.UserEmail == cashierEmail);
            var manager = users.FirstOrDefault(u => u.UserEmail == managerEmail);

            if (cashier == null)
                return (false, "Cashier not found.");
            if (manager == null)
                return (false, "Manager not found.");

            // Get the valid promo (only available promos that are not expired)
            var promo = await _dataContext.CouponPromo
                .FirstOrDefaultAsync(p => p.PromoCode == promoCode
                                       && p.IsAvailable
                                       && (p.ExpirationTime == null || p.ExpirationTime > now));

            if (promo == null)
                return (false, "Invalid or expired Promo Code.");

            // Fetch the current pending order along with its items
            var currentOrder = await _dataContext.Order
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.IsPending);

            if (currentOrder == null)
                return (false, "No pending order found. Please start an order before applying a promo discount.");
            if (!string.IsNullOrEmpty(currentOrder.DiscountType))
                return (false, "A discount has already been applied to this order. No additional discount can be applied.");
            if (currentOrder.Promo != null)
                return (false, "A promo discount has already been applied to this order.");
            if (currentOrder.Coupon != null && currentOrder.Coupon.Any())
                return (false, "A coupon has already been applied to this order. You cannot combine promo discounts with coupons.");


            // Wrap updates in a transaction for atomicity
            using (var transaction = await _dataContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // Apply promo details to the current order
                    currentOrder.Promo = promo;
                    currentOrder.TotalAmount -= promo.PromoAmount ?? 0m;
                    currentOrder.DiscountType = DiscountTypeEnum.Promo.ToString();
                    currentOrder.DiscountAmount = promo.PromoAmount;

                    // Mark the promo as used
                    promo.IsAvailable = false;
                    promo.UpdatedAt = now;

                    await _dataContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return (false, $"Error applying promo: {ex.Message}");
                }
            }

            return (true, $"{promo.PromoAmount}");
        }


        public async Task<(bool, string)> VoidOrderItem(VoidOrderItemDTO voidOrder)
        {
            // Fetch cashier and manager in a single query
            var users = await _dataContext.User
                .Where(u => (u.UserEmail == voidOrder.cashierEmail || u.UserEmail == voidOrder.managerEmail) && u.IsActive)
                .ToListAsync();

            var cashier = users.FirstOrDefault(u => u.UserEmail == voidOrder.cashierEmail);
            var manager = users.FirstOrDefault(u => u.UserEmail == voidOrder.managerEmail);

            if (cashier == null)
            {
                return (false, "Cashier not found.");
            }
            if (manager == null)
            {
                return (false, "Unauthorized Card!");
            }

            decimal discountedPrice = 0m;

            // Fetch item and related items in a single query
            var voidItem = await _dataContext.Item
                .Include(i => i.Order)
                    .ThenInclude(o => o.Items)
                .Include(i => i.Meal)
                .Where(i => i.EntryId == voidOrder.entryId &&
                            i.Order.Cashier.UserEmail == voidOrder.cashierEmail &&
                            i.Order.IsPending)
                .FirstOrDefaultAsync();

            if (voidItem == null)
            {
                return (false, "Item not found.");
            }

            // Mark the main item as void
            voidItem.IsVoid = true;
            voidItem.VoidedAt = DateTimeOffset.Now;
            discountedPrice = ((voidItem.ItemPrice ?? 0m) * (voidItem.ItemQTY ?? 0)) * 0.20m;

            // Void the main item and related drinks/add-ons
            var relatedItems = await _dataContext.Item
                .Where(i => i.Meal != null && i.Meal.Id == voidItem.Id && i.Order.Id == voidItem.Order.Id)
                .ToListAsync();

            // If related items exist, mark them as void as well
            if (relatedItems.Count > 0)
            {
                foreach (var item in relatedItems)
                {
                    item.IsVoid = true;
                    item.VoidedAt = DateTimeOffset.Now;
                    discountedPrice += ((item.ItemPrice ?? 0m) * (item.ItemQTY ?? 0)) * 0.20m;
                }
            }

            // Recalculate and update the order's TotalAmount after voiding items.
            var order = voidItem.Order;
            order.TotalAmount = order.Items
                .Where(i => !i.IsVoid)
                .Sum(i => (i.ItemPrice ?? 0m) * (i.ItemQTY ?? 1));

            // Recalculate discount for non-void items.
            if (order.DiscountType != null)
            {
                var discountItems = order.Items.Where(i => !i.IsVoid && (i.IsPwdDiscounted || i.IsSeniorDiscounted));
                // Recalculate discount amount (here assuming a 20% discount).
                order.DiscountAmount = discountItems
                    .Sum(i => ((i.ItemPrice ?? 0m) * (i.ItemQTY ?? 1)) * 0.20m);
                order.EligiblePwdScCount = discountItems.Count();

                // Limit the list entries to the eligible count (remove extra items from the beginning)
                int eligibleCount = order.EligiblePwdScCount ?? 0;
                var voidName = (order.EligiblePwdScNames?.Split(", ") ?? Array.Empty<string>()).ToList();
                var voidOSca = (order.OSCAIdsNum?.Split(", ") ?? Array.Empty<string>()).ToList();

                if (voidName.Count > eligibleCount)
                    voidName.RemoveRange(0, voidName.Count - eligibleCount);
                if (voidOSca.Count > eligibleCount)
                    voidOSca.RemoveRange(0, voidOSca.Count - eligibleCount);

                order.EligiblePwdScNames = string.Join(", ", voidName);
                order.OSCAIdsNum = string.Join(", ", voidOSca);



                // Optionally, if no items remain with a discount, clear the discount type.
                if (!discountItems.Any())
                {
                    order.DiscountType = null;
                    order.DiscountAmount = 0m;
                    order.EligiblePwdScCount = 0;
                }
            }

            await _dataContext.SaveChangesAsync();
            return (true, relatedItems.Count == 0 ? "Solo item voided." : "Meal and related items voided.");
        }

    }
}
