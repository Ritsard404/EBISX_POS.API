using System.Text.Json.Serialization;

namespace EBISX_POS.API.Services.DTO.Order
{
    public class AddPwdScDiscountDTO
    {
        public int PwdScCount { get; set; }
        public bool IsSeniorDisc { get; set; }
        public required string ManagerEmail { get; set; }
        [JsonIgnore]
        public string? CashierEmail { get; set; }
        public required List<string> EntryId { get; set; }
    }
}
