using DiscountSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Common;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Vendor> Vendors { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
