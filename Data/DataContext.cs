using EBISX_POS.API.Models;
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

        // ✅ Auto-calculate subtotal before saving
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Item>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Entity.ItemSubTotal = (decimal)entry.Entity.ItemQTY * entry.Entity.ItemPrice;
                }
            }
            return base.SaveChanges();
        }
    }
}
