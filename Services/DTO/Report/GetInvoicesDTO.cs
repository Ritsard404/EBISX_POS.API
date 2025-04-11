namespace EBISX_POS.API.Services.DTO.Report
{
    public class GetInvoicesDTO
    {
        public required long InvoiceNum { get; set; }
        public required string DateTime { get; set; }
        public required string CashierEmail { get; set; }
        public required string CashierName { get; set; }
    }
}
