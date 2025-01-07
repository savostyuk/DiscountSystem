using DiscountSystem.Domain.Common;

namespace DiscountSystem.Domain.Entities;

public class Discount : BaseAuditableEntity
{
    public string Condition { get; set; }
    public string Promocode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid VendorId { get; set; }
    public Vendor Vendor { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}
