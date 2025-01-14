﻿using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Vendors.Queries;

public record GetAllVendorsQuery : IRequest<List<VendorShortDTO>>
{
}

public class GetAllVendorsHandler : IRequestHandler<GetAllVendorsQuery, List<VendorShortDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllVendorsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VendorShortDTO>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Vendors
            .Select(v => new VendorShortDTO
            {
                Id = v.Id,
                VendorName = v.VendorName,
            }).ToListAsync(cancellationToken);
    }
}