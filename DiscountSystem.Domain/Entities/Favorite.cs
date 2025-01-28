using DiscountSystem.Domain.Common;

namespace DiscountSystem.Domain.Entities;

public class Favorite : BaseAuditableEntity
{
    public string? Note { get; set; }

    public Guid DiscountId { get; set; }
    public Discount Discount { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}
