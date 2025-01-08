using DiscountSystem.Domain.Entities;

namespace DiscountSystem.Application.Discounts.Queries;

public class DiscountDTO
{
    public Guid Id { get; set; }
    public string Condition { get; set; }
    public string Promocode { get; set; }
    public string VendorName { get; set; }
    public Guid CategoryId { get; set; }
    public string Category { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<Guid> Tags { get; set; }
}
