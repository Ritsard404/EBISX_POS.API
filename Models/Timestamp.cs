using System.ComponentModel.DataAnnotations;

namespace EBISX_POS.API.Models
{
    public class Timestamp
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset? TsIn { get; set; }
        public DateTimeOffset? TsOut { get; set; }
        public DateTimeOffset? TsBreakOut { get; set; }
        public DateTimeOffset? TsBreakIn { get; set; }
        public required User User { get; set; }
    }
}
    