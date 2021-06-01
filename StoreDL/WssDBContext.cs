using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class WssDBContext : IdentityDbContext<User, UserRole, Guid>
    {
        public WssDBContext(DbContextOptions options) : base(options)
        {
        }

        protected WssDBContext()
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Location>()
                .Property(location => location.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>()
                .Property(product => product.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Inventory>()
                .Property(inventory => inventory.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<LineItem>()
                .Property(lineItem => lineItem.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>()
                .Property(order => order.Id)
                .ValueGeneratedOnAdd();
        }
    }
}