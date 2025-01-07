using DiscountSystem.Application.Common;
using DiscountSystem.Application.Discounts.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Categories.Queries;

public record GetDiscountByCategoryIdQuery : IRequest<List<DiscountDTO>>
{
    public Guid CategoryId { get; set; }
    public GetDiscountByCategoryIdQuery(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}

public class GetDiscountByCategoryIdHandler : IRequestHandler<GetDiscountByCategoryIdQuery, List<DiscountDTO>>
{

    private readonly IApplicationDbContext _context;
    public GetDiscountByCategoryIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DiscountDTO>> Handle(GetDiscountByCategoryIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Discounts
            .Where(d => d.CategoryId == request.CategoryId)
            .Select(d => new DiscountDTO
            {
                Id = d.Id,
                Condition = d.Condition,
                Promocode = d.Promocode,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            }).ToListAsync(cancellationToken);
    }
}
