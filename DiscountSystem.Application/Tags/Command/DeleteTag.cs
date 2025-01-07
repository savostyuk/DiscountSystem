using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Tags.Commands;

public record DeleteTagCommand(Guid Id) : IRequest;

public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags.FindAsync([request.Id], cancellationToken);

        _context.Tags.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}