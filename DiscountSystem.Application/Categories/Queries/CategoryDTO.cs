namespace DiscountSystem.Application.Categories.Queries;

public class CategoryDTO
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
