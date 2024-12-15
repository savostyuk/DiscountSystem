using DiscountSystem.Domain.Enums;

namespace DiscountSystem.Infrastructure;

public static class RoleExtensions
{
    public static string ToIdentityRole (this ApplicationRole role)
    {
        return role.ToString();
    }
}
