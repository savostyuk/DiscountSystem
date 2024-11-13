using DiscountSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace DiscountSystem.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Role { get; set; }

}
