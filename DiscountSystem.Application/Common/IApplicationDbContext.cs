using DiscountSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Common;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Vendor> Vendors { get; }
    DbSet<Category> Categories { get; }
    DbSet<Discount> Discounts { get; }
    DbSet<Favorite> Favorites { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
