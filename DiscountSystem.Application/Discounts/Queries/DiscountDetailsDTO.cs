namespace DiscountSystem.Application.Discounts.Queries;

public class DiscountDetailsDTO
{
    public Guid Id { get; set; }
    public string Condition { get; set; }
    public string Promocode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string VendorName { get; set; }
    public string Website { get; set; }
    public string WorkingHours { get; set; }
    public string Phone { get; set; }

    public string CategoryName { get; set; }
    public Guid CategoryId { get; set; }
    public IList<Guid> Tags { get; set; }
    public bool IsFavorite { get; set; }
    public string? Note { get; set; }
}
