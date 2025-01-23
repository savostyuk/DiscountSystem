using DiscountSystem.Application.Discounts.Queries;

namespace DiscountSystem.Application.Favorites.Queries;

public class FavoriteDiscountDTO
{
    public string? Note { get; set; }
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Condition { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Promocode { get; set; }
    public string VendorName { get; set; }
    public IList<Guid> Tags { get; set; }
    public bool IsFavorite { get; set; }
    public DiscountDTO Discount { get; set; }
}
