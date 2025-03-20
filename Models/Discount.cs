using EBISX_POS.API.Models.Utils;
using System.ComponentModel.DataAnnotations;

namespace EBISX_POS.API.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public required string DiscountType { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? DiscountPromoPercent { get; set; }
        public int? EligiblePwdScCount { get; set; }
        public string? PromoCode { get; set; }
        public string? CouponCode { get; set; }
        public DateTimeOffset? ExpirationTime { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    }
}
