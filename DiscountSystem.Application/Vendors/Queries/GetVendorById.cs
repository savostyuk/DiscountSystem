using DiscountSystem.Application.Common;
using DiscountSystem.Application.Discounts.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Vendors.Queries;

public record GetVendorByIdQuery : IRequest<VendorDTO>
{
    public Guid Id { get; set; }
}

public class GetVendorByIdQueryHandler : IRequestHandler<GetVendorByIdQuery, VendorDTO>
{
    private readonly IApplicationDbContext _context;

    public GetVendorByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<VendorDTO> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
    {
        var vendorDetails = await _context.Vendors
             .Where(v => v.Id == request.Id)
             .Include(v => v.Discounts)
                 .ThenInclude(d => d.Tags)
             .Select(v => new VendorDTO
             {
                 Id = v.Id,
                 Email = v.Email,
                 VendorName = v.VendorName,
                 WorkingHours = v.WorkingHours,
                 Website = v.Website,
                 Phone = v.Phone,
                 Address = v.Address,
                 Discounts = v.Discounts.Select(d => new DiscountDTO
                 {
                     Id = d.Id,
                     Condition = d.Condition,
                     Promocode = d.Promocode,
                     StartDate = d.StartDate,
                     EndDate = d.EndDate,
                     CategoryId = d.CategoryId,
                     Tags = d.Tags.Select(tag => tag.Id).ToList() // Get only the tag IDs
                 }).ToList()
             }).FirstOrDefaultAsync(cancellationToken);

        if (vendorDetails == null)
        {
            throw new Exception($"Vendor with Id {request.Id} was not foumd");
        }

        return vendorDetails;
    }
}