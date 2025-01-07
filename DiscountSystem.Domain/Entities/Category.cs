using DiscountSystem.Domain.Common;

namespace DiscountSystem.Domain.Entities;

public class Category : BaseAuditableEntity
{
    public string CategoryName { get; set; }
    public ICollection<Tag> Tags { get; set; }
}
