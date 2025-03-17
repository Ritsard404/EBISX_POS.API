using EBISX_POS.API.Data;
using EBISX_POS.API.Models;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EBISX_POS.API.Services.Repositories
{
    public class OrderRepository(DataContext _dataContext) : IOrder
    {

        // Adds an item to the pending order for the specified cashier.
        // If no pending order exists, a new order is created.
        public async Task<(bool, string)> AddOrderItem(int menuId, string cashierEmail)
        {
            // Find a pending order for the active cashier.
            var currentOrder = await _dataContext.Order
                .Include(o => o.Cashier)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.IsPending
                    && o.Cashier != null
                    && o.Cashier.UserEmail == cashierEmail
                    && o.Cashier.IsActive);

            // If no pending order exists, create a new one.
            if (currentOrder == null)
            {
                // Retrieve the cashier object.
                var cashier = await _dataContext.User.FirstOrDefaultAsync(u => u.UserEmail == cashierEmail);
                if (cashier == null)
                {
                    return (false, "Cashier not found.");
                }

                // Create a new Order.
                currentOrder = new Order
                {
                    OrderType = "", // Set an appropriate order type if needed.
                    TotalAmount = 0m,
                    CreatedAt = DateTimeOffset.Now,
                    Cashier = cashier,
                    IsPending = true,
                    IsCancelled = false,
                    Items = new List<Item>()
                };

                await _dataContext.Order.AddAsync(currentOrder);
            }

            // Retrieve the Menu item using the provided menuId.
            var menu = await _dataContext.Menu.FirstOrDefaultAsync(m => m.Id == menuId);
            if (menu == null)
            {
                return (false, "Menu item not found.");
            }

            // Create a new Item for the order.
            var newItem = new Item
            {
                ItemQTY = 1, // Default quantity; adjust as needed.
                ItemPrice = menu.MenuPrice, // Assuming Menu has a MenuPrice property.
                Menu = menu,
                Order = currentOrder,
                createdAt = DateTimeOffset.Now
            };

            // Add the new item to the order.
            currentOrder.Items.Add(newItem);

            // Update the total amount for the order.
            currentOrder.TotalAmount += (newItem.ItemPrice ?? 0m) * (newItem.ItemQTY ?? 1);

            // Save changes to the database.
            await _dataContext.SaveChangesAsync();

            return (true, "Order item added.");
        }
    }
}
