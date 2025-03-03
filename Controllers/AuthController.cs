using EBISX_POS.API.Services.DTO.Auth;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EBISX_POS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuth _auth): ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> Cashiers()
        {
            var cashiers = await _auth.Cashiers();
            return Ok(cashiers);
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentCashier()
        {
            if (Request.Cookies.TryGetValue("CashierEmail", out var cashierEmail))
            {
                return Ok(new { cashierEmail });
            }

            return Unauthorized(new { message = "No active cashier session" });
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogInDTO logInDTO)
        {
            var (success, cashierEmail, cashierName) = await _auth.LogIn(logInDTO);
            if (!success)
                return Unauthorized("Invalid Credential!");

            // Set HTTP-only cookie containing the email
            Response.Cookies.Append("CashierEmail", cashierEmail, new CookieOptions
            {
                HttpOnly = true,  // Prevents JavaScript access (Prevents XSS)
                Secure = true,    // Only send cookie over HTTPS
                SameSite = SameSiteMode.Strict, // Restricts cross-site requests
                Expires = DateTimeOffset.Now.AddDays(1) // Cookie expires in 8 hours
            });

            return Ok(new { cashierName });
        }
    }
}
