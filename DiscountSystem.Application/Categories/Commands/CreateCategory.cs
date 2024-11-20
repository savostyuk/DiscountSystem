using DiscountSystem.Application.Common;
using DiscountSystem.Domain.Entities;
using MediatR;

namespace DiscountSystem.Application.Categories.Commands;

public class CreateCategoryCommand : IRequest<Guid>
{ 
    public string CategoryName { get; set; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Category
        {
            CategoryName = request.CategoryName,
        };

        _context.Categories.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
