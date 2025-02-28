using EBISX_POS.API.Services.DTO.Auth;

namespace EBISX_POS.API.Services.Interfaces
{
    public interface IAuth
    {
        Task<(bool, string, string)> LogIn(LogInDTO logInDTO);
        Task<List<CashierDTO>> Cashiers();
    }
}
