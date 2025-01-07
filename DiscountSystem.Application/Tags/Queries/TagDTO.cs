namespace DiscountSystem.Application.Tags.Queries;

public class TagDTO
{
    public Guid Id { get; set; }
    public string TagName { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
}
