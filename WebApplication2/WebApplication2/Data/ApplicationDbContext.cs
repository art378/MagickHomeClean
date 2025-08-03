using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Моделі
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Subcategory> Subcategories { get; set; } = null!;
        public DbSet<ProductSubcategory> ProductSubcategories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Discount> Discounts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Багато-до-багатьох: Product ↔️ Subcategory
            modelBuilder.Entity<ProductSubcategory>()
                .HasKey(ps => new { ps.ProductId, ps.SubcategoryId });

            modelBuilder.Entity<ProductSubcategory>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSubcategories)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<ProductSubcategory>()
                .HasOne(ps => ps.Subcategory)
                .WithMany(s => s.ProductSubcategories)
                .HasForeignKey(ps => ps.SubcategoryId);

            // Тип decimal → точність 18,2 (для ціни)
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }
    }
}