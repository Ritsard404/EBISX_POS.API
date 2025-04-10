namespace EBISX_POS.API.Services.Interfaces
{
    public interface IReport
    {
        Task<(string CashInDrawer, string CurrentCashDrawer)> CashTrack(string cashierEmail);
    }
}
