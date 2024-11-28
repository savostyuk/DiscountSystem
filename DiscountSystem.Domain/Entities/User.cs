using DiscountSystem.Domain.Common;
using DiscountSystem.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace DiscountSystem.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public Roles Role { get; set; }

    public ICollection<Favorite> Favorites { get; set; }
}
