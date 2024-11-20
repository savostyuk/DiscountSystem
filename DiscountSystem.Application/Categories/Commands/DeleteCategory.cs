using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Categories.Commands;

public record DeleteCategoryCommand (Guid Id) : IRequest;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FindAsync([request.Id], cancellationToken);

        _context.Categories.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
