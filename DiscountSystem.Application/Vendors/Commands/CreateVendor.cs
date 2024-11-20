using DiscountSystem.Application.Common;
using DiscountSystem.Domain.Entities;
using MediatR;

namespace DiscountSystem.Application.Vendors.Commands;

public class CreateVendorCommand : IRequest<Guid>
{
    public string VendorName { get; set; }
    public string WorkingHours { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
}

public class CreateVendorCommandHandler : IRequestHandler<CreateVendorCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateVendorCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = new Vendor
        {
            VendorName = request.VendorName,
            WorkingHours = request.WorkingHours,
            Website = request.Website,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
        };

        _context.Vendors.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
