using Beyti.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Beyti.Data
{
    public class BeytiDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public BeytiDbContext(DbContextOptions<BeytiDbContext> options)
      : base(options) { }

        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<ChefProfile> ChefProfiles => Set<ChefProfile>();
        public DbSet<SupplierProfile> SupplierProfiles => Set<SupplierProfile>();
        public DbSet<DeliveryProfile> DeliveryProfiles => Set<DeliveryProfile>();
        public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();

        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<Supply> Supplies => Set<Supply>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<WalletTransaction> WalletTransactions => Set<WalletTransaction>();

        public DbSet<Review> Reviews => Set<Review>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Wallet
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Address
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Chef)
                .WithMany()
                .HasForeignKey(o => o.ChefId)
                .OnDelete(DeleteBehavior.NoAction);

            // OrderDetails
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Recipe)
                .WithMany()
                .HasForeignKey(od => od.RecipeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Wallet Transactions
            modelBuilder.Entity<WalletTransaction>()
                .HasOne(t => t.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WalletId);

            // Reviews
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.FromCustomerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ToUser)
                .WithMany()
                .HasForeignKey(r => r.ToUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
