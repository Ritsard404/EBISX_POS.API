﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace EBISX_POS.API.Models
{
    public class CouponPromo
    {
        [Key]
        public int Id { get; set; }
        public required string Description { get; set; }
        public string? PromoCode { get; set; }
        public string? CouponCode { get; set; }
        public decimal? PromoAmount { get; set; }
        public ICollection<Menu>? CouponMenus { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTimeOffset? ExpirationTime { get; set; }

        // Timestamps for audit purposes
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
