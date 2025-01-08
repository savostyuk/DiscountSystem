using DiscountSystem.Application.Common;
using DiscountSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Discounts.Commands;

public record UpdateDiscountCommand : IRequest
{
    public Guid Id { get; init; }
    public string Condition { get; init; }
    public string Promocode { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public Guid CategoryId { get; set; }
    public ICollection<Guid> Tags { get; set; }
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
        var entity = await _context.Discounts
            .Include(d => d.Tags)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity with Id = {request.Id} was not found");
        }

        entity.Condition = request.Condition;
        entity.Promocode = request.Promocode;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.CategoryId = request.CategoryId;

        if (request.Tags != null && request.Tags.Any())
        {
            var validTags = await _context.Tags
                .Where(tag => request.Tags.Contains(tag.Id))
                .ToListAsync(cancellationToken);

            if (validTags.Count != request.Tags.Count)
            {
                throw new Exception("One or more provided TagIds are invalid.");
            }

            entity.Tags.Clear();

            foreach (var tag in validTags)
            {
                entity.Tags.Add(tag);
            }
        }
        else
        {
            entity.Tags.Clear();
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
