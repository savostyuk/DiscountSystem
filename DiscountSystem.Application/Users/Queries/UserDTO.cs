using DiscountSystem.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace DiscountSystem.Application.Users.Queries;

public class UserDTO : BaseAuditableEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}
