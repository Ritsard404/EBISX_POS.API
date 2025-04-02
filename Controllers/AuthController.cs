using EBISX_POS.API.Services.DTO.Auth;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EBISX_POS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuth _auth) : ControllerBase
    {
        [HttpGet()]
        public async Task<IActionResult> Cashiers()
        {
            var cashiers = await _auth.Cashiers();
            return Ok(cashiers);
        }

        [HttpGet()]
        public async Task<IActionResult> GetCashier()
        {
            string? cashierEmail = Request.Cookies["CashierEmail"];
            return Ok(cashierEmail);
        }

        [HttpGet]
        public async Task<IActionResult> HasPendingOrder()
        {
            if (!Request.Cookies.TryGetValue("CashierEmail", out var cashierEmail) || string.IsNullOrWhiteSpace(cashierEmail))
            {
                var (success, pendingCashierEmail, cashierName) = await _auth.HasPendingOrder();
                if (success)
                {
                    Response.Cookies.Append("CashierEmail", pendingCashierEmail, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.Now.AddDays(1)
                    });

                    return Ok(new { cashierName });
                }

                return Unauthorized(new { message = "No active cashier session and no pending orders" });
            }

            return Ok(new { cashierEmail });
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
                Secure = false,    // Only send cookie over HTTPS
                SameSite = SameSiteMode.Lax, // Restricts cross-site requests
                //Expires = DateTimeOffset.Now.AddMinutes(1) 
                Expires = DateTimeOffset.Now.AddDays(1)
            });

            return Ok(new { cashierName });
        }

        [HttpPut]
        public async Task<IActionResult> LogOut(string managerEmail)
        {
            string? cashierEmail = Request.Cookies["CashierEmail"];
            if (cashierEmail == null)
                return Unauthorized(new { message = "No active cashier session" });

            var (success, message) = await _auth.LogOut(new LogInDTO() { CashierEmail = cashierEmail, ManagerEmail = managerEmail });
            if (!success)
                return BadRequest(message);

            // Clear the HTTP-only cookie by removing it.
            Response.Cookies.Delete("CashierEmail");

            return Ok(new { message });
        }
    }
}
