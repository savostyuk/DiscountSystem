using DiscountSystem.Domain.Enums;

namespace DiscountSystem.Infrastructure;

public static class RoleExtensions
{
    //Метод расширения используется всего один раз, его можно убрать, и в AuthController просто использовать
    //ToString() у ролей при необходимости
    public static string ToIdentityRole(this ApplicationRole role)
    {
        return role.ToString();
    }
}
