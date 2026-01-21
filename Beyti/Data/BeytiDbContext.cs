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

            /* =========================
               User - Address (One to One)
               ========================= */
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)
                .WithOne(a => a.User)
                .HasForeignKey<Address>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            /* =========================
               User - Wallet (One to One)
               ========================= */
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            /* =========================
               User - Profiles (One to One)
               ========================= */

            modelBuilder.Entity<User>()
                .HasOne<SupplierProfile>()
                .WithOne(sp => sp.User)
                .HasForeignKey<SupplierProfile>(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne<ChefProfile>()
                .WithOne(cp => cp.User)
                .HasForeignKey<ChefProfile>(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne<CustomerProfile>()
                .WithOne(cp => cp.User)
                .HasForeignKey<CustomerProfile>(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne<DeliveryProfile>()
                .WithOne(dp => dp.User)
                .HasForeignKey<DeliveryProfile>(dp => dp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            /* =========================
               SupplierProfile - Supplies
               ========================= */
            modelBuilder.Entity<Supply>()
                .HasOne(s => s.SupplierProfile)
                .WithMany(sp => sp.Supplies)
                .HasForeignKey(s => s.SupplierProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            /* =========================
               ChefProfile - Recipes
               ========================= */
            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.ChefProfile)
                .WithMany(cp => cp.Recipes)
                .HasForeignKey(r => r.ChefProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            /* =========================
               Order - User (Customer / Chef)
               ========================= */
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Chef)
                .WithMany()
                .HasForeignKey(o => o.ChefId)
                .OnDelete(DeleteBehavior.Restrict);

            /* =========================
               Order - OrderDetails
               ========================= */
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Recipe)
                .WithMany()
                .HasForeignKey(od => od.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);

            /* =========================
               Wallet - Transactions
               ========================= */
            modelBuilder.Entity<WalletTransaction>()
                .HasOne(t => t.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            /* =========================
               Reviews
               ========================= */
            modelBuilder.Entity<Review>()
                .HasOne(r => r.CustomerProfile)
                .WithMany(cp => cp.Reviews)
                .HasForeignKey(r => r.CustomerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.ToUser)
                .WithMany()
                .HasForeignKey(r => r.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
