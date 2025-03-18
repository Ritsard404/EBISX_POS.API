using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBISX_POS.API.Models
{
    public class Timestamp
    {
        [Key]
        public int Id { get; set; }

        // The time when the cashier clocked in.
        public DateTimeOffset? TsIn { get; set; }

        // The time when the cashier clocked out.
        public DateTimeOffset? TsOut { get; set; }

        // The time when the cashier started a break.
        public DateTimeOffset? TsBreakOut { get; set; }

        // The time when the cashier ended a break.
        public DateTimeOffset? TsBreakIn { get; set; }

        // The cashier associated with this timestamp record (required).
        [Required]
        public required virtual User Cashier { get; set; }

        // The manager who authorized the clock-in (required).
        [Required]
        public User ManagerIn { get; set; }

        // The manager who authorized the clock-out.
        public User? ManagerOut { get; set; }

        // The manager who authorized the end of break.
        public User? ManagerBreakIn { get; set; }

        // The manager who authorized the start of break.
        public User? ManagerBreakOut { get; set; }

        // Computes the total break duration if both break times are provided.
        [NotMapped]
        public TimeSpan? TotalBreakDuration =>
            (TsBreakIn.HasValue && TsBreakOut.HasValue) ? TsBreakIn.Value - TsBreakOut.Value : null;

        // Computes the net work duration (clock-out minus clock-in, minus break duration if provided).
        [NotMapped]
        public TimeSpan? NetWorkDuration
        {
            get
            {
                if (TsIn.HasValue && TsOut.HasValue)
                {
                    var totalDuration = TsOut.Value - TsIn.Value;
                    if (TotalBreakDuration.HasValue)
                    {
                        totalDuration -= TotalBreakDuration.Value;
                    }
                    return totalDuration;
                }
                return null;
            }
        }
    }
}
