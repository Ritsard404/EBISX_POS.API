using EBISX_POS.API.Services.DTO.Order;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBISX_POS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController(IOrder _order) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddCurrentOrderVoid(AddCurrentOrderVoidDTO voidOrder)
        {
            //string? cashierEmail = Request.Cookies["CashierEmail"];

            //if (cashierEmail == null)
            //{
            //    return Unauthorized(new { message = "No active cashier session" });
            //}


            //addOrder.cashierEmail = cashierEmail;
            voidOrder.cashierEmail = "user2@example.com";

            var (success, message) = await _order.AddCurrentOrderVoid(voidOrder);
            if (success)
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem(AddOrderDTO addOrder)
        {
            //string? cashierEmail = Request.Cookies["CashierEmail"];

            //if (cashierEmail == null)
            //{
            //    return Unauthorized(new { message = "No active cashier session" });
            //}


            //addOrder.cashierEmail = cashierEmail;
            addOrder.cashierEmail = "user2@example.com";

            var (success, message) = await _order.AddOrderItem(addOrder);
            if (success)
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

        [HttpPut]
        public async Task<IActionResult> VoidOrderItem(VoidOrderItemDTO voidOrder)
        {
            //string? cashierEmail = Request.Cookies["CashierEmail"];
            //if (cashierEmail == null)
            //{
            //    return Unauthorized(new { message = "No active cashier session" });
            //}
            //voidOrder.cashierEmail = cashierEmail;
            voidOrder.cashierEmail = "user2@example.com";
            voidOrder.managerEmail = "user1@example.com";

            var (success, message) = await _order.VoidOrderItem(voidOrder);
            if (success)
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

        [HttpPut]
        public async Task<IActionResult> EditQtyOrderItem(EditOrderItemQuantityDTO editOrder)
        {
            //string? cashierEmail = Request.Cookies["CashierEmail"];
            //if (cashierEmail == null)
            //{
            //    return Unauthorized(new { message = "No active cashier session" });
            //}
            editOrder.cashierEmail = "user2@example.com";

            var (success, message) = await _order.EditQtyOrderItem(editOrder);
            if (success)
            {
                return Ok(message);
            }
            return BadRequest(message);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentOrderItems()
        {
            //// Retrieve cashier email from cookies (if enabled)
            //string? cashierEmail = Request.Cookies["CashierEmail"];

            //// Check if the cashier session exists
            //if (string.IsNullOrEmpty(cashierEmail))
            //{
            //    return Unauthorized(new { message = "No active cashier session." });
            //}


            string cashierEmail = "user2@example.com";

            // Get current order items using cashierEmail
            var currentOrderItems = await _order.GetCurrentOrderItems(cashierEmail);

            // Return the list (empty if no items found)
            return Ok(currentOrderItems);
        }

    }
}
