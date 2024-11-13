using DiscountSystem.Application;
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
    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating (modelBuilder);
    }
}
