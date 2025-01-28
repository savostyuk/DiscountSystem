using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Categories.Commands;

public record UpdateCategoryCommand : IRequest
{
    public Guid Id { get; init; }
    public string CategoryName { get; init; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FindAsync([request.Id], cancellationToken);

        if (entity == null) //Ипользовать Guard, чтобы обеспечить консистентность сообщений в проекте
        {
            throw new Exception($"Entity with Id = {request.Id} was not found");
        }

        entity.CategoryName = request.CategoryName;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
