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
                .Where(m => m.UserEmail == logInDTO.ManagerEmail && m.UserRole != "Cashier")
                .FirstOrDefaultAsync();

            var cashier = await _dataContext.User
                .Where(m => m.UserEmail == logInDTO.CashierEmail && m.UserRole == "Cashier")
                .FirstOrDefaultAsync();

            if (manager == null || cashier == null)
                return (false, "Invalid Credential!", "");

            return (true, cashier.UserEmail, cashier.UserFName + " " + cashier.UserLName);
        }
    }
}
