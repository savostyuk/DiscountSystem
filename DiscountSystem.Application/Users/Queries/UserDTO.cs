namespace DiscountSystem.Application.Users.Queries;

public class UserDTO
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Location { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
