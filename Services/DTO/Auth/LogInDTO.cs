namespace EBISX_POS.API.Services.DTO.Auth
{
    public class LogInDTO
    {
        public required string ManagerEmail { get; set; }
        public required string CashierEmail { get; set; }

    }
}
