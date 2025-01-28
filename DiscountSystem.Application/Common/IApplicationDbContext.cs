using DiscountSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Common;

public interface IApplicationDbContext //Есть уже пара интерфейсов, можно их перенести в папку Interfaces
{
    DbSet<User> Users { get; }
    DbSet<Vendor> Vendors { get; }
    DbSet<Category> Categories { get; }
    DbSet<Tag> Tags { get; }
    DbSet<Discount> Discounts { get; }
    DbSet<Favorite> Favorites { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
