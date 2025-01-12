using DiscountSystem.Application.Discounts.Queries;
using DiscountSystem.Domain.Entities;

namespace DiscountSystem.Application.Favorites.Queries;

public class FavoriteDTO
{
    public string? Note { get; set; }
    public Guid DiscountId { get; set; }
    public DiscountDTO Discount { get; set; }
}
