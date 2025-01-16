using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiscountSystem.Application.Favorites.Queries;

public class GetFavoritesByUserIdQuery : IRequest<List<FavoriteDiscountDTO>>
{
}

public class GetFavoritesByUserIdQueryHandler : IRequestHandler<GetFavoritesByUserIdQuery, List<FavoriteDiscountDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetFavoritesByUserIdQueryHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<FavoriteDiscountDTO>> Handle(GetFavoritesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }


        var userGuid = Guid.Parse(userId);

        var userFavorites = await _context.Favorites
            .Where(f => f.UserId == userGuid)
            .Select(f => f.DiscountId)
            .ToListAsync(cancellationToken);

        return await _context.Favorites
            .Where(f => f.UserId == userGuid)
            .Include(f => f.Discount)
                .ThenInclude(d => d.Vendor) // Include Vendor details
            .Include(f => f.Discount)
                .ThenInclude(d => d.Category) // Include Category details
            .Include(f => f.Discount)
                .ThenInclude(d => d.Tags) // Include Tags details
            .Select(f => new FavoriteDiscountDTO
            {
                Note = f.Note,
                Id = f.DiscountId,
                CategoryId = f.Discount.CategoryId,
                CategoryName = f.Discount.Category.CategoryName,
                StartDate = f.Discount.StartDate,
                Condition = f.Discount.Condition,
                Promocode = f.Discount.Promocode,
                VendorName = f.Discount.Vendor.VendorName, // Populated Vendor object
                Tags = f.Discount.Tags.Select(tag => tag.Id).ToList(), // Populated Tags list
                IsFavorite = userFavorites.Contains(f.Id),
            }).ToListAsync(cancellationToken);
    }
}
