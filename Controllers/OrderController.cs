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
    }
}
