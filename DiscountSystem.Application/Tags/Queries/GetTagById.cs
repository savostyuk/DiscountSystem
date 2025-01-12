using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Tags.Queries;

public record GetTagByIdQuery : IRequest<TagDTO>
{
    public Guid Id { get; set; }
}

public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, TagDTO>
{
    private readonly IApplicationDbContext _context;
    public GetTagByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TagDTO> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
    {
        var tagDetails = await _context.Tags
           .Where(t => t.Id == request.Id)
           .Select(t => new TagDTO
           {
               Id = t.Id,
               TagName = t.TagName
           }).FirstOrDefaultAsync(cancellationToken);

        if (tagDetails == null)
        {
            throw new Exception($"Tag with Id {request.Id} was not found");
        }

        return tagDetails;
    }
}
