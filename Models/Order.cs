﻿using System.ComponentModel.DataAnnotations;

namespace EBISX_POS.API.Models
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        public required string OrderType { get; set; }
        public required decimal TotalAmount { get; set; }
        public decimal? CashTendered { get; set; }
        public required DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
        public bool IsCancelled { get; set; } = false;
        public bool IsPending { get; set; } = true;

        public string? DiscountType { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? EligiblePwdScCount { get; set; }
        public string? EligiblePwdScNames { get; set; }
        public string? OSCAIdsNum { get; set; }
        public ICollection<CouponPromo> Coupon { get; set; } = new List<CouponPromo>();
        public CouponPromo? Promo { get; set; }

        public required User Cashier { get; set; }
        public User? Manager { get; set; }

        // Navigation property for related Items
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<AlternativePayments> AlternativePayments { get; set; } = new List<AlternativePayments>();
    }
}
