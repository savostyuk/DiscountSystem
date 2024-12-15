using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Discounts.Queries;

public record GetAllDiscountsQuery : IRequest<List<DiscountDTO>>
{
}

public class GetAllDiscountsHandler : IRequestHandler<GetAllDiscountsQuery, List<DiscountDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllDiscountsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DiscountDTO>> Handle(GetAllDiscountsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Discounts
            .Select(d => new DiscountDTO
            {
                Id = d.Id,
                DiscountName = d.DiscountName,
                Condition = d.Condition,
                Promocode = d.Promocode,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                CreatedAt = d.CreatedAt,
                CreatedBy = d.CreatedBy,
                LastModifiedAt = d.LastModifiedAt,
                LastModifiedBy = d.LastModifiedBy
            }).ToListAsync(cancellationToken);
    }
}