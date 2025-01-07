using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Categories.Queries;

public record GetCategoryByIdQuery : IRequest<CategoryDTO>
{
    public Guid Id { get; set; }
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDTO>
{
    private readonly IApplicationDbContext _context;

    public GetCategoryByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDTO> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var categoryDetails = await _context.Categories
            .Where(c => c.Id == request.Id)
            .Select(c => new CategoryDTO
            {
                Id = c.Id,
                CategoryName = c.CategoryName
            }).FirstOrDefaultAsync(cancellationToken);

        if (categoryDetails == null)
        {
            throw new Exception($"Category with Id {request.Id} was not foumd");
        }

        return categoryDetails;
    }
}
