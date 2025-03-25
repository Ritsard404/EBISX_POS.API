using System.Text.Json.Serialization;

namespace EBISX_POS.API.Services.DTO.Order
{
    public class VoidOrderItemDTO
    {
        public string entryId { get; set; }
        public required string managerEmail { get; set; }

        [JsonIgnore]
        public string? cashierEmail { get; set; }
    }
}
