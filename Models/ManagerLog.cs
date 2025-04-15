using System.ComponentModel.DataAnnotations;

namespace EBISX_POS.API.Models
{
    public class ManagerLog
    {
        [Key]
        public int Id { get; set; }

        public Timestamp? Timestamp { get; set; }
        public required User Manager { get; set; }
        public required string Action { get; set; }
        public decimal WithdrawAmount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
