using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Discounts.Queries;

public record GetDiscountDetailsQuery : IRequest<DiscountDetailsDTO>
{
    public Guid Id { get; set; }
}

public class GetDiscountDetailsHandler : IRequestHandler<GetDiscountDetailsQuery, DiscountDetailsDTO>
{
    private readonly IApplicationDbContext _context;

    public GetDiscountDetailsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DiscountDetailsDTO> Handle(GetDiscountDetailsQuery request, CancellationToken cancellationToken)
    {
        var ticketDetails = await _context.Discounts
            .Where(d => d.Id == request.Id)
            .Select(d => new DiscountDetailsDTO
            {
                Id = d.Id,
                DiscountName = d.DiscountName,
                Condition = d.Condition,
                Promocode = d.Promocode,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                VendorName = d.Vendor.VendorName,
                CategoryName = d.Category.CategoryName,
            }).FirstOrDefaultAsync(cancellationToken);

        if (ticketDetails == null) 
        {
            throw new Exception($"Discount with Id {request.Id} was not foumd");
        }

        return ticketDetails;
    }
}
