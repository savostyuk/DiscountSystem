using DiscountSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace DiscountSystem.Domain.Entities;

public class Vendor : BaseAuditableEntity
{
    public string VendorName {  get; set; }
    public string? WorkingHours { get; set; }
    public string? Website { get; set; }
    [EmailAddress]
    public string? Email { get; set; }

    public string? Phone { get; set; }
    public string? Address { get; set; }
    public ICollection<Discount>? Discounts { get; set; }
}
