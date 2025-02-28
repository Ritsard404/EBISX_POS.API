using EBISX_POS.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBISX_POS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuController(IMenu _menu) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var categories = await _menu.Categories();
            return Ok(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Menus(int ctgryId)
        {
            var menus = await _menu.Menus(ctgryId);
            return Ok(menus);
        }
    }
}
