using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Vendors.Queries;

public record GetAllVendorsQuery : IRequest<List<VendorDTO>>
{
}

public class GetAllVendorsHandler : IRequestHandler<GetAllVendorsQuery, List<VendorDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllVendorsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VendorDTO>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Vendors
            .Select(v => new VendorDTO
            {
                Id = v.Id,
                Email = v.Email,
                VendorName = v.VendorName,
                WorkingHours = v.WorkingHours,
                Website = v.Website,
                Phone = v.Phone,
                Address = v.Address,
            }).ToListAsync(cancellationToken);
    }
}