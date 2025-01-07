using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Tags.Queries;

public record GetTagsByCategoryIdQuery : IRequest<List<TagDTO>>
{
    public Guid CategoryId { get; set; }
    public GetTagsByCategoryIdQuery(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}

public class GetTagsByCategoryIdQueryHandler : IRequestHandler<GetTagsByCategoryIdQuery, List<TagDTO>>
{
    private readonly IApplicationDbContext _context;
    public GetTagsByCategoryIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<TagDTO>> Handle(GetTagsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Where(d => d.CategoryId == request.CategoryId)
            .Select(d => new TagDTO
            {
                Id = d.Id,
                TagName = d.TagName,
                CategoryId = d.CategoryId,
            }).ToListAsync(cancellationToken);
    }
}
