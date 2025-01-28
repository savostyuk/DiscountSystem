using DiscountSystem.Application.Common;
using DiscountSystem.Domain.Entities;
using MediatR;

namespace DiscountSystem.Application.Tags.Commands; //Исправить название папки на Commands

public class CreateTagCommand : IRequest<Guid>
{
    public string TagName { get; set; }
    public Guid CategoryId { get; set; }
}

public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var entity = new Tag
        {
            TagName = request.TagName,
            CategoryId = request.CategoryId,
        };

        _context.Tags.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}