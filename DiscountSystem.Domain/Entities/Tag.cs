using DiscountSystem.Domain.Common;

namespace DiscountSystem.Domain.Entities;

public class Tag : BaseAuditableEntity
{
    public string TagName { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
}
