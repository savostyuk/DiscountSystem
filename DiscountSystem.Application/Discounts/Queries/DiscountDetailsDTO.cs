namespace DiscountSystem.Application.Discounts.Queries;

public class DiscountDetailsDTO
{
    public Guid Id { get; set; }
    public string DiscountName { get; set; }
    public string Condition { get; set; }
    public string Promocode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string VendorName { get; set; }
    public string CategoryName { get; set; }
}
