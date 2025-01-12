using DiscountSystem.Application.Common;
using DiscountSystem.Application.Common.Extensions;
using DiscountSystem.Application.Common.Models;
using MediatR;

namespace DiscountSystem.Application.Discounts.Queries;

public record GetPaginatedDiscountsQuery : IRequest<PaginatedList<DiscountDTO>>
{
    public int PageSize { get; init; } = 10;
    public int PageNumber { get; init; } = 1;
}

public class GetPaginatedDiscountsQueryHandler : IRequestHandler<GetPaginatedDiscountsQuery, PaginatedList<DiscountDTO>>
{
    private readonly IApplicationDbContext _context;
    public GetPaginatedDiscountsQueryHandler(IApplicationDbContext context)
    {
        _context = context;   
    }

    public async Task<PaginatedList<DiscountDTO>> Handle(GetPaginatedDiscountsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Discounts
            .Select(d => new DiscountDTO
            {
                Id = d.Id,
                VendorName = d.Vendor.VendorName,
                CategoryName = d.Category.CategoryName,
                Condition = d.Condition,
                Promocode = d.Promocode,
            }).ToPaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
