﻿using EBISX_POS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EBISX_POS.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Timestamp> Timestamp { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<DrinkType> DrinkType { get; set; }
        public DbSet<AddOnType> AddOnType { get; set; }
        public DbSet<CouponPromo> CouponPromo { get; set; }
        public DbSet<SaleType> SaleType { get; set; }
        public DbSet<AlternativePayments> AlternativePayments { get; set; }
        public DbSet<PosTerminalInfo> PosTerminalInfo { get; set; }
        public DbSet<ManagerLog> ManagerLog { get; set; }

        // ✅ Auto-calculate subtotal before saving
        //public override int SaveChanges()
        //{
        //    foreach (var entry in ChangeTracker.Entries<Item>())
        //    {
        //        if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.ItemSubTotal = (decimal)entry.Entity.ItemQTY * entry.Entity.ItemPrice;
        //        }
        //    }
        //    return base.SaveChanges();
        //}
    }
}
