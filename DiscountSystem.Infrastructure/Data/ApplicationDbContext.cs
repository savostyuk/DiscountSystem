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
    }
}
