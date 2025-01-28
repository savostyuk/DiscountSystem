using DiscountSystem.Domain.Entities;

namespace DiscountSystem.Application.Categories.Queries;

public class CategoryDTO
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public ICollection<Tag> Tags { get; set; }
}
