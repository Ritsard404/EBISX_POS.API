using EBISX_POS.API.Data;
using EBISX_POS.API.Services.DTO.Auth;
using EBISX_POS.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EBISX_POS.API.Services.Repositories
{
    public class AuthRepository(DataContext _dataContext) : IAuth
    {

        public async Task<List<CashierDTO>> Cashiers()
        {
            var cashiers = await _dataContext.User
                .Where(u => u.UserRole == "Cashier")
                .Select(u => new CashierDTO
                {
                    Email = u.UserEmail,
                    Name = u.UserFName + " " + u.UserLName
                })
                .ToListAsync();

            return cashiers;
        }

        public async Task<(bool, string, string)> LogIn(LogInDTO logInDTO)
        {
            var manager = await _dataContext.User
                .Where(m => m.UserEmail == logInDTO.managerEmail && m.UserRole != "Cashier")
                .FirstOrDefaultAsync();

            var cashier = await _dataContext.User
                .Where(m => m.UserEmail == logInDTO.cashierEmail && m.UserRole == "Cashier")
                .FirstOrDefaultAsync();

            if (manager == null || cashier == null)
                return (false, "Invalid Credential!", "");

            return (true, cashier.UserEmail, cashier.UserFName + " " + cashier.UserLName);

        }
    }
}
