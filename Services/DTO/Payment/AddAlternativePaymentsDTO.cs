namespace EBISX_POS.API.Services.DTO.Payment
{
    public class AddAlternativePaymentsDTO
    {
        public required string Reference { get; set; }
        public required decimal Amount { get; set; }
        public required int SaleTypeId { get; set; }
    }
}
