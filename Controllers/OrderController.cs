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
        public async Task<IActionResult> AddOrderItem(int menuId, string cashierEmail)
        {
            var (success, message) = await _order.AddOrderItem(menuId, cashierEmail);
            if (success)
            {
                return Ok(message);
            }
            return BadRequest(message);
        }
    }
}
