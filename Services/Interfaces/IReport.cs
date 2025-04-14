using EBISX_POS.API.Services.DTO.Report;

namespace EBISX_POS.API.Services.Interfaces
{
    public interface IReport
    {
        Task<(string CashInDrawer, string CurrentCashDrawer)> CashTrack(string cashierEmail);
        Task<List<GetInvoicesDTO>> GetInvoicesByDate(DateTime dateTime);
        Task<List<GetInvoiceDTO>> GetInvoiceById(long invId);
        Task<XInvoiceReportDTO> XInvoiceReport();
    }
}
