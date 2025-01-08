using DiscountSystem.Application.Common;
using DiscountSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Discounts.Commands;

public class CreateDiscountCommand : IRequest<Guid>
{
    public string Condition { get; set; }
    public string Promocode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid VendorId { get; set; }
    public Guid CategoryId { get; set; }
    public ICollection<Guid> Tags { get; set; }
}

public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateDiscountCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
    {

        var tags = await _context.Tags
            .Where(tag => request.Tags.Contains(tag.Id))
            .ToListAsync(cancellationToken);

        if (tags.Count != request.Tags.Count)
        {
            throw new Exception("One or more provided TagIds are invalid.");
        }

        var entity = new Discount
        {
            Condition = request.Condition,
            Promocode = request.Promocode,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            VendorId = request.VendorId,
            CategoryId = request.CategoryId,
            Tags = tags
        };

        _context.Discounts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
