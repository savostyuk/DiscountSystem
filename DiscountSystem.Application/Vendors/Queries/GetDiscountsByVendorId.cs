using DiscountSystem.Application.Common;
using DiscountSystem.Application.Discounts.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Vendors.Queries;

public record GetDiscountByVendorIdQuery : IRequest<List<DiscountDTO>>
{
    public Guid VendorId { get; set; }

    public GetDiscountByVendorIdQuery(Guid vendorId)
    {
        VendorId = vendorId;
    }
}

public class GetDiscountByVendorIdHandler : IRequestHandler<GetDiscountByVendorIdQuery, List<DiscountDTO>>
{

    private readonly IApplicationDbContext _context;
    public GetDiscountByVendorIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DiscountDTO>> Handle(GetDiscountByVendorIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Discounts
            .Where(d => d.VendorId == request.VendorId)
            .Select(d => new DiscountDTO
            {
                Id = d.Id,
                Condition = d.Condition,
                Promocode = d.Promocode,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            }).ToListAsync(cancellationToken);
    }
}
