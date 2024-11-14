using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Vendors.Commands;

public record DeleteVendorCommand (Guid Id) : IRequest;

public class DeleteVendorCommandHadler : IRequestHandler<DeleteVendorCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteVendorCommandHadler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Vendors.FindAsync([request.Id], cancellationToken);   

        _context.Vendors.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
