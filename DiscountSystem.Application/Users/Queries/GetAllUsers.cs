using DiscountSystem.Application.Common;
using DiscountSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DiscountSystem.Application.Users.Queries;

public record GetAllUsersQuery : IRequest<List<UserDTO>>
{
}

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public GetAllUsersHandler(IApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = _userManager.Users.ToList();
        var userWithRoles = new List<UserDTO>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userWithRoles.Add(new UserDTO
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Location = user.Location,
                Email = user.Email,
                Role = roles.First()
            });
        }

        return userWithRoles;
    }
}
