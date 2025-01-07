using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Categories.Queries;

public record GetAllCategoriesQuery : IRequest<List<CategoryDTO>>;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllCategoriesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Tags = c.Tags
            }).ToListAsync(cancellationToken);
    }
}
