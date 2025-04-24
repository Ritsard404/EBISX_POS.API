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
    public class AuthRepository(DataContext _dataContext) : IAuth
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

        public async Task<(bool, string)> CashWithdrawDrawer(string cashierEmail, string managerEmail, decimal cash)
        {
            var timestamp = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Include(t => t.ManagerLog)
                .FirstOrDefaultAsync(t => t.Cashier.UserEmail == cashierEmail && t.TsOut == null);

            var manager = await _dataContext.User
                .FirstOrDefaultAsync(m => m.UserEmail == managerEmail && m.UserRole != "Cashier" && m.IsActive);


            if (timestamp?.CashInDrawerAmount is not { } available)
                return (false, "No active session or drawer amount not set.");

            var tsIn = timestamp.TsIn;

            decimal totalCashInDrawer = await _dataContext.Order
                    .Where(o =>
                        o.Cashier.UserEmail == cashierEmail &&
                        !o.IsCancelled &&
                        !o.IsPending &&
                        !o.IsReturned &&
                        o.CreatedAt >= tsIn &&
                        o.CashTendered != null &&
                        o.TotalAmount != 0
                    )
                    .SumAsync(o =>
                        o.CashTendered!.Value - o.ChangeAmount!.Value
                    );

            if (cash > available + totalCashInDrawer)
                return (false, "Withdrawal exceeds drawer balance.");

            if (manager is null)
                return (false, "Invalid manager credentials.");

            timestamp.ManagerLog.Add(new UserLog
            {
                Manager = manager,
                WithdrawAmount = cash,
                Timestamp = timestamp,
                Action = "Withdrawal"
            });

            await _dataContext.SaveChangesAsync();
            return (true, "Cash withdrawal recorded.");
        }


        public async Task<(bool, string, string)> HasPendingOrder()
        {
            // Check if there's a pending order with a related cashier.
            var pendingOrder = await _dataContext.Order
                .Include(o => o.Cashier)
                .Where(o => o.IsPending)
                .FirstOrDefaultAsync();

            // Check if there's an active timestamp (meaning the cashier hasn't timed out yet).
            var activeTimestamp = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Where(t => t.TsOut == null)
                .FirstOrDefaultAsync();

            // If a pending order exists, use its cashier information.
            if (pendingOrder != null)
            {
                return (true,
                        pendingOrder.Cashier.UserEmail,
                        $"{pendingOrder.Cashier.UserFName} {pendingOrder.Cashier.UserLName}");
            }
            // If no pending order but the cashier is still clocked in, return its information.
            else if (activeTimestamp != null)
            {
                return (true,
                        activeTimestamp.Cashier.UserEmail,
                        $"{activeTimestamp.Cashier.UserFName} {activeTimestamp.Cashier.UserLName}");
            }

            // If neither condition is met, return false with empty values.
            return (false, "", "");
        }

        public async Task<bool> IsCashedDrawer(string cashierEmail)
        {

            var timestamp = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Where(t => t.Cashier.UserEmail == cashierEmail && t.TsOut == null && t.CashInDrawerAmount != null && t.CashInDrawerAmount >= 1000)
                .FirstOrDefaultAsync();

            return timestamp != null;
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


            var pendingOrder = await _dataContext.Order
                .Where(o => o.IsPending && o.Cashier == cashier)
                .AnyAsync();

            if (pendingOrder)
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

        public async Task<(bool, string)> SetCashInDrawer(string cashierEmail, decimal cash)
        {
            var timestamp = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Where(t => t.Cashier.UserEmail == cashierEmail && t.TsOut == null)
                .FirstAsync();

            timestamp.CashInDrawerAmount = cash;

            if (timestamp.CashInDrawerAmount == null)
                return (false, "Cash in drawer amount is null!");

            await _dataContext.SaveChangesAsync();

            return (true, "Cash in drawer set!");
        }

        public async Task<(bool, string)> SetCashOutDrawer(string cashierEmail, decimal cash)
        {
            var timestamp = await _dataContext.Timestamp
                .Include(t => t.Cashier)
                .Where(t => t.Cashier.UserEmail == cashierEmail && t.TsOut == null)
                .FirstAsync();

            timestamp.CashOutDrawerAmount = cash;

            if (timestamp.CashInDrawerAmount == null)
                return (false, "Cash in drawer amount is null!");

            await _dataContext.SaveChangesAsync();

            return (true, "Cash out drawer set!");
        }
    }
}
