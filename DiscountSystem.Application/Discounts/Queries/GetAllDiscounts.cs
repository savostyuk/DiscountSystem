using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiscountSystem.Application.Discounts.Queries;

public record GetAllDiscountsQuery : IRequest<List<DiscountDTO>>
{
}

public class GetAllDiscountsHandler : IRequestHandler<GetAllDiscountsQuery, List<DiscountDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllDiscountsHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<DiscountDTO>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
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

        return await _context.Discounts
            .Select(d => new DiscountDTO
            {
                Id = d.Id,
                VendorName = d.Vendor.VendorName,
                CategoryName = d.Category.CategoryName,
                CategoryId = d.Category.Id,
                Condition = d.Condition,
                Promocode = d.Promocode,
                Tags = d.Tags.Select(tag => tag.Id).ToList(),
                IsFavorite = userFavorites.Contains(d.Id)
            }).ToListAsync(cancellationToken);
    }
}
