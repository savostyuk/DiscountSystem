using DiscountSystem.Application.Common;
using DiscountSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) 
    {
    }  
    
    public DbSet<User> Users {get; set;}
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating (modelBuilder);

        modelBuilder.Entity<Discount>()
            .HasOne(d => d.Vendor)
            .WithMany(d => d.Discounts)
            .HasForeignKey(d => d.VendorId);

        modelBuilder.Entity<Discount>()
            .HasOne(d => d.Category)
            .WithMany()
            .HasForeignKey(d => d.CategoryId);

        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.UserId, f.DiscountId });

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Discount)
            .WithMany()
            .HasForeignKey(f => f.DiscountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
