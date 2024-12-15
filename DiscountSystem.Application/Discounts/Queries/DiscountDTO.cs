namespace DiscountSystem.Application.Discounts.Queries;

public class DiscountDTO
{
    public Guid Id { get; set; }
    public string DiscountName { get; set; }
    public string Condition { get; set; }
    public string Promocode { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
