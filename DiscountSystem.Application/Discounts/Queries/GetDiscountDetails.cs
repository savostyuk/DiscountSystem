using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiscountSystem.Application.Discounts.Queries;

public record GetDiscountDetailsQuery : IRequest<DiscountDetailsDTO>
{
    public Guid Id { get; set; }
}

public class GetDiscountDetailsHandler : IRequestHandler<GetDiscountDetailsQuery, DiscountDetailsDTO>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetDiscountDetailsHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor; 
    }

    public async Task<DiscountDetailsDTO> Handle(GetDiscountDetailsQuery request, CancellationToken cancellationToken)
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

        var discountDetails = await _context.Discounts
            .Where(d => d.Id == request.Id)
            .Select(d => new DiscountDetailsDTO
            {
                Id = d.Id,
                Condition = d.Condition,
                Promocode = d.Promocode,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                VendorName = d.Vendor.VendorName,
                Website = d.Vendor.Website,
                Phone = d.Vendor.Phone,
                WorkingHours = d.Vendor.WorkingHours,
                CategoryId = d.CategoryId,
                CategoryName = d.Category.CategoryName,
                Tags = d.Tags.Select(tag => tag.Id).ToList(),
                IsFavorite = _context.Favorites
                .Any(f => f.UserId == userGuid && f.DiscountId == d.Id),
                Note = _context.Favorites
                .Where(f => f.UserId == userGuid && f.DiscountId == d.Id)
                .Select(f => f.Note)
                .FirstOrDefault()
            }).FirstOrDefaultAsync(cancellationToken);

        if (discountDetails == null) 
        {
            throw new Exception($"Discount with Id {request.Id} was not foumd");
        }

        return discountDetails;
    }
}
