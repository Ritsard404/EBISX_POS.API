using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace EBISX_POS.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public required string OrderType { get; set; }
        public required decimal TotalAmount { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required User  User { get; set; }
    }
}
