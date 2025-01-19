using Microsoft.AspNetCore.Identity;

namespace DiscountSystem.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Location { get; set; }
    public override string UserName
    {
        get => Email; // Use Email as the UserName
        set => base.UserName = value; // Set UserName internally
    }
    public ICollection<Favorite> Favorites { get; set; }
}
