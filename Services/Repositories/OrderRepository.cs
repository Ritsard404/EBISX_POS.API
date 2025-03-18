using EBISX_POS.API.Data;
using EBISX_POS.API.Models;
using EBISX_POS.API.Services.DTO.Order;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

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
            currentOrder.TotalAmount += totalAmount;

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

            decimal totalAmount = 0m;

            void AddItem(Menu? item, decimal? customPrice = null, Item? parentMeal = null)
            {
                if (item == null) return;

                var newItem = new Item
                {
                    ItemQTY = addOrder.qty,
                    ItemPrice = customPrice ?? item.MenuPrice,
                    Menu = parentMeal == null ? item : null,
                    Drink = parentMeal == null && item == drink ? item : null,
                    AddOn = parentMeal == null && item == addOn ? item : null,
                    Meal = parentMeal,
                    Order = currentOrder
                };

                currentOrder.Items.Add(newItem);
                totalAmount += (newItem.ItemPrice ?? 0m) * (newItem.ItemQTY ?? 1);
            }

            var mealItem = menu != null ? new Item
            {
                ItemQTY = addOrder.qty,
                ItemPrice = menu.MenuPrice,
                Menu = menu,
                Order = currentOrder
            } : null;

            if (mealItem != null)
            {
                currentOrder.Items.Add(mealItem);
                totalAmount += (mealItem.ItemPrice ?? 0m) * (mealItem.ItemQTY ?? 1);
            }

            AddItem(drink, addOrder.drinkPrice > 0 ? addOrder.drinkPrice : drink?.MenuPrice, mealItem);
            AddItem(addOn, addOrder.addOnPrice > 0 ? addOrder.addOnPrice : addOn?.MenuPrice, mealItem);

            currentOrder.TotalAmount += totalAmount;

            await _dataContext.SaveChangesAsync();

            return (true, "Order item added.");
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

            // Fetch the item with the associated order
            var item = await _dataContext.Item
                .Include(i => i.Order)
                .Include(i => i.Meal)
                .FirstOrDefaultAsync(i =>
                    i.Id == editOrder.itemId &&
                    i.Order.Cashier.UserEmail == editOrder.cashierEmail &&
                    i.Order.IsPending &&
                    !i.IsVoid
                );

            //  Return if item is not found or voided
            if (item == null)
            {
                return (false, "Item not found or cannot be modified.");
            }

            // Validate quantity (must be greater than 0)
            if (editOrder.qty <= 0)
            {
                return (false, "Quantity must be greater than zero.");
            }

            // Update item quantity and adjust total amount
            int oldQty = item.ItemQTY ?? 1;
            decimal itemPrice = item.ItemPrice ?? 0m;

            item.ItemQTY = editOrder.qty;

            // Recalculate the total amount in the order
            item.Order.TotalAmount += (itemPrice * (editOrder.qty - oldQty));

            // Check if the item is a parent (Meal)
            if (item.Meal == null)
            {
                // Update child items if the item is a parent
                var childItems = await _dataContext.Item
                    .Where(i => i.Meal != null && i.Meal.Id == item.Id && !i.IsVoid)
                    .ToListAsync();

                foreach (var childItem in childItems)
                {
                    childItem.ItemQTY = editOrder.qty;
                    item.Order.TotalAmount += (childItem.ItemPrice ?? 0m) * (editOrder.qty - oldQty);
                }
            }

            // Save changes to the database
            await _dataContext.SaveChangesAsync();

            return (true, $"Quantity updated to {editOrder.qty}.");
        }


        public async Task<List<GetCurrentOrderItemsDTO>> GetCurrentOrderItemsDTOs(string cashierEmail)
        {
            // Check if the cashier is valid and active
            var cashier = await _dataContext.User
                .FirstOrDefaultAsync(u => u.UserEmail == cashierEmail && u.IsActive);

            // Return an empty list if no cashier is found
            if (cashier == null) return new List<GetCurrentOrderItemsDTO>();

            // Retrieve all pending orders for the cashier with related items
            var currentOrderItems = await _dataContext.Order
                .Include(o => o.Items)
                    .ThenInclude(i => i.Menu)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Drink)
                .Include(o => o.Items)
                    .ThenInclude(i => i.AddOn)
                .Include(o => o.Cashier)
                .Where(o => o.IsPending &&
                            o.Cashier != null &&
                            o.Cashier.UserEmail == cashierEmail &&
                            o.Cashier.IsActive)
                .Select(o => new GetCurrentOrderItemsDTO
                {
                    SubOrders = o.Items
                        .Where(i => !i.IsVoid) // Exclude voided items
                        .Select(i => new CurrentOrderItemsSubOrder
                        {
                            MenuId =i.Menu.Id,
                            DrinkId = i.Drink.Id,
                            AddOnId = i.AddOn.Id,
                            Name = i.Menu.MenuName,
                            Size = i.Menu.Size,
                            ItemPrice = i.ItemPrice ?? 0m,
                            Quantity = i.ItemQTY ?? 1,
                            IsFirstItem = i.Meal == null // True for main item
                        })
                        .ToList()
                })
                .ToListAsync();

            // Return the list of DTOs
            return currentOrderItems;
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

            // Fetch item and related items in a single query
            var voidItem = await _dataContext.Item
                .Include(i => i.Order)
                .Include(i => i.Meal)
                .Where(i => i.Id == voidOrder.itemId &&
                            i.Order.Cashier.UserEmail == voidOrder.cashierEmail &&
                            i.Order.IsPending)
                .FirstOrDefaultAsync();

            if (voidItem == null)
            {
                return (false, "Item not found.");
            }

            // If it's a solo item, void it immediately
            if (voidItem.Meal == null)
            {
                voidItem.IsVoid = true;
                voidItem.VoidedAt = DateTimeOffset.Now;
                await _dataContext.SaveChangesAsync();
                return (true, "Solo item voided.");
            }

            // Void the main item and related drinks/add-ons
            var relatedItems = await _dataContext.Item
                .Where(i => i.Meal != null && i.Meal.Id == voidItem.Id && i.Order.Id == voidItem.Order.Id)
                .ToListAsync();

            // Mark all items (main + related) as void
            var itemsToVoid = new List<Item> { voidItem };
            itemsToVoid.AddRange(relatedItems);

            foreach (var item in itemsToVoid)
            {
                item.IsVoid = true;
                item.VoidedAt = DateTimeOffset.Now;
            }

            await _dataContext.SaveChangesAsync();
            return (true, voidItem.Meal == null ? "Solo item voided." : "Meal and related items voided.");
        }
    }
}
