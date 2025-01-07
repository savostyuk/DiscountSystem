using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Tags.Commands;

public record UpdateTagCommand : IRequest
{
    public Guid Id { get; init; }
    public string TagName { get; init; }
}

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags.FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity with Id = {request.Id} was not found");
        }

        entity.TagName = request.TagName;

        await _context.SaveChangesAsync(cancellationToken);
    }
}