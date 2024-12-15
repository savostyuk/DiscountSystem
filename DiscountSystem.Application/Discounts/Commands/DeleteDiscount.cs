using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Discounts.Commands;

public record DeleteDiscountCommand (Guid Id) : IRequest;

public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteDiscountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Discounts.FindAsync([request.Id], cancellationToken);

        _context.Discounts.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
