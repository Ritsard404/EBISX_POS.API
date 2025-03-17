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
        public required DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public bool IsCancelled { get; set; } = false;
        public bool IsPending { get; set; } = true;
        public required User Cashier { get; set; }
        public User? Manager { get; set; }

        // Navigation property for related Items
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
