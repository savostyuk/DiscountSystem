using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Users.Commands;

public record DeleteUserCommand (Guid Id) : IRequest;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FindAsync([request.Id], cancellationToken);

        _context.Users.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
