using DiscountSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DiscountSystem.Application.Users.Commands;

public record UpdateUserRoleCommand: IRequest
{
    public Guid Id { get; init; }
    public string NewRole { get; init; }
}

public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UpdateUserRoleCommandHandler(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user == null)
        {
            throw new Exception($"User with Id = {request.Id} was not found.");
        }

        if (!await _roleManager.RoleExistsAsync(request.NewRole))
        {
            throw new Exception($"Role '{request.NewRole}' does not exist.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!removeResult.Succeeded)
        {
            throw new Exception($"Failed to remove user with Id = {request.Id} from existing roles.");
        }

        var addResult = await _userManager.AddToRoleAsync(user, request.NewRole);

        if (!addResult.Succeeded)
        {
            throw new Exception($"Failed to add user with Id = {request.Id} to the new role.");
        }
    }
}
