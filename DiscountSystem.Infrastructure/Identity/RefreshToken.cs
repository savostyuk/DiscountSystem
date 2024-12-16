using DiscountSystem.Domain.Entities;

namespace DiscountSystem.Infrastructure.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
}
