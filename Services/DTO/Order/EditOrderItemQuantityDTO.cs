using System.Text.Json.Serialization;

namespace EBISX_POS.API.Services.DTO.Order
{
    public class EditOrderItemQuantityDTO
    {
        public long entryId { get; set; }
        public int qty { get; set; }
        [JsonIgnore]
        public string? cashierEmail { get; set; }
    }
}
