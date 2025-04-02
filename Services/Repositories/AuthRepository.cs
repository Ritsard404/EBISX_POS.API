using EBISX_POS.API.Data;
using EBISX_POS.API.Models;
using EBISX_POS.API.Services.DTO.Auth;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace EBISX_POS.API.Services.Repositories
{
    public class AuthRepository(DataContext _dataContext, IConfiguration _config) : IAuth
    {
        public async Task<List<CashierDTO>> Cashiers()
        {
            var cashiers = await _dataContext.User
                .Where(u => u.UserRole == "Cashier" && u.IsActive)
                .Select(u => new CashierDTO
                {
                    Email = u.UserEmail,
                    Name = u.UserFName + " " + u.UserLName
                })
                .ToListAsync();

            return cashiers;
        }

        public async Task<(bool, string, string)> HasPendingOrder()
        {
            var pendingOrder = await _dataContext.Order
                .Include(c => c.Cashier)
                .Where(o => o.IsPending)
                .FirstOrDefaultAsync();

            if (pendingOrder == null)
                return (false, "", "");

            var notTimeOut = await _dataContext.Timestamp
                .Where(t => t.TsOut == null && t.Cashier == pendingOrder.Cashier)
                .FirstOrDefaultAsync();

            if (notTimeOut != null)
                return (false, "", "");

            return (true, pendingOrder.Cashier.UserEmail, pendingOrder.Cashier.UserFName + " " + pendingOrder.Cashier.UserLName);
        }

        public async Task<(bool, string, string)> LogIn(LogInDTO logInDTO)
        {
            var manager = await _dataContext.User
                .Where(m => m.UserEmail == logInDTO.ManagerEmail && m.UserRole != "Cashier" && m.IsActive)
                .FirstOrDefaultAsync();

            var cashier = await _dataContext.User
                .Where(m => m.UserEmail == logInDTO.CashierEmail && m.UserRole == "Cashier" && m.IsActive)
                .FirstOrDefaultAsync();

            if (manager == null || cashier == null)
                return (false, "Invalid Credential!", "");

            // Create a new Timestamp record for the cashier's login ("time in")
            var timestamp = new Timestamp
            {
                TsIn = DateTimeOffset.Now, // Set the time-in to now
                Cashier = cashier,
                ManagerIn = manager      // Manager who authorized the login
            };

            _dataContext.Timestamp.Add(timestamp);
            await _dataContext.SaveChangesAsync();

            return (true, cashier.UserEmail, cashier.UserFName + " " + cashier.UserLName);
        }

        public async Task<(bool, string)> LogOut(LogInDTO logOutDTO)
        {
            var manager = await _dataContext.User
                .Where(m => m.UserEmail == logOutDTO.ManagerEmail && m.UserRole != "Cashier" && m.IsActive)
                .FirstOrDefaultAsync();

            var cashier = await _dataContext.User
                .Where(m => m.UserEmail == logOutDTO.CashierEmail && m.UserRole == "Cashier" && m.IsActive)
                .FirstOrDefaultAsync();

            if (manager == null)
                return (false, "Invalid Credential!");
            if (cashier == null)
                return (false, "Invalid Credential of Cashier!");

            var (success, pendingCashierEmail, cashierName) = await HasPendingOrder();

            if (success)
                return (false, "Cashier has pending order!");

            var timestamp = await _dataContext.Timestamp
                .Where(t => t.Cashier == cashier && t.TsOut == null)
                .FirstOrDefaultAsync();

            if (timestamp == null)
                return (false, "Cashier is not clocked in!");

            timestamp.TsOut = DateTimeOffset.UtcNow; // Set the time-out to now
            timestamp.ManagerOut = manager; // Manager who authorized the logout

            await _dataContext.SaveChangesAsync();

            return (true, "Cashier Logged Out!");
        }
    }
}
