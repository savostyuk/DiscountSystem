using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Discounts.Commands;

public record UpdateDiscountCommand : IRequest
{
    public Guid Id { get; init; }
    public string DiscountName { get; init; }
    public string Condition { get; init; }
    public string Promocode { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}

public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateDiscountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Discounts.FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity with Id = {request.Id} was not found");
        }

        entity.DiscountName = request.DiscountName;
        entity.Condition = request.Condition;
        entity.Promocode = request.Promocode;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
