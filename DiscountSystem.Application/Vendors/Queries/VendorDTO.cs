﻿using DiscountSystem.Domain.Common;
using DiscountSystem.Domain.Entities;

namespace DiscountSystem.Application.Vendors.Queries;

public class VendorDTO : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public string VendorName { get; set; }
    public string WorkingHours { get; set; }
    public string Website { get; set; }
    public string Email { get; set; }

    public string Phone { get; set; }
    public string Address { get; set; }
    public ICollection<Discount> Discounts { get; set; }
}
