using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Vendors.Commands;

public record UpdateVendorCommand : IRequest
{
    public Guid Id { get; init; }
    public string VendorName { get; init; }
    public string WorkingHours { get; init; }
    public string Website { get; init; }
    public string Email { get; init; }

    public string Phone { get; init; }
    public string Address { get; init; }
}

public class UpdateVendorCommandHandler : IRequestHandler<UpdateVendorCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateVendorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vendors.FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity with Id = {request.Id} was not found");
        }

        entity.VendorName = request.VendorName;
        entity.WorkingHours = request.WorkingHours;
        entity.Website = request.Website;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.Address = request.Address;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
