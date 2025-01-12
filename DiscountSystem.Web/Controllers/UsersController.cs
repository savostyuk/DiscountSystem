using DiscountSystem.Application.Common.Models;
using DiscountSystem.Application.Tags.Queries;
using DiscountSystem.Application.Users;
using DiscountSystem.Application.Users.Commands;
using DiscountSystem.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator,
        ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    /// <summary>
    /// Get all users.
    /// </summary>
    /// <response code="204">Returns the list of users.</response>
    /// <response code="404">If no users are found.</response>
    /// <returns>A list of all users.</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsers() 
    {
        _logger.LogInformation("Received request to get all users.");

        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query);

        if (users is null or []) 
        {
            _logger.LogWarning("No users found.");
            return NotFound("No user found!");
        }

        _logger.LogInformation("Successfully retrieved {UserCount} users.", users.Count);
        return Ok(users);
    }

    /// <summary>
    /// Get a paginated list of users.
    /// </summary>
    /// <param name="query">The query parameters for pagination.</param>
    /// <response code="200">Returns a paginated list of users.</response>
    /// <response code="404">If no users are found for the specified query.</response>
    /// <returns>A paginated list of users.</returns>
    [AllowAnonymous]
    [HttpGet("paginated")]
    public async Task<PaginatedList<UserDTO>> GetUsersWithPagination([FromQuery] GetPaginatedUsersQuery query)
    {
        _logger.LogInformation("Received request to get page of users.");

        return await _mediator.Send(query);
    }

    /// <summary>
    /// Get users based on a specified filter.
    /// </summary>
    /// <param name="filter">The filter criteria to apply.</param>
    /// <response code="200">Returns the list of users matching the filter criteria.</response>
    /// <response code="404">If no users are found matching the filter criteria.</response>
    /// <returns>A filtered list of users.</returns>
    [AllowAnonymous]
    [HttpGet("filter")]
    public async Task<ActionResult<List<UserDTO>>> GetUsersWithFilter([FromQuery] UserFilter filter)
    {
        var query = new GetFilteredUsersQuery { Filter = filter };
        var users = await _mediator.Send(query);

        if(users is null or [])
        {
            return NotFound("Users were not found.");
        }

        return Ok(users);
    }

    /// <summary>
    /// Get a user by its unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <response code="200">User successfully retrieved.</response>
    /// <response code="404">If the user is not found.</response>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        var query = new GetUserDetailsQuery { Id = userId };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Delete an user by Id.
    /// </summary>
    /// <param name="Id">The id of the user.</param>
    /// <response code="204">User successfully deleted.</response>
    /// <response code="404">If the user is not found.</response>
    /// <returns></returns>
    [HttpDelete("{Id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid Id)
    {
        _logger.LogInformation("Received request to delete user with Id: {UserId}.", Id);
        var command = new DeleteUserCommand(Id);
        try
        {
            await _mediator.Send(command);
            _logger.LogInformation("Successfully deleted user with Id: {UserId}.", Id);
            return NoContent();
        }
        catch (Exception ex) 
        {
            _logger.LogWarning(ex, "User with Id: {UserId} not found.", Id);
            return NotFound(ex.Message);
        }

    }

    /// <summary>
    /// Update an existing user by Id.
    /// </summary>
    /// <param name="Id">The id of the user.</param>
    /// <response code="204">User successfully updated.</response>
    /// <response code="404">If the user is not found.</response>
    /// <response code="400">If the user request is invalid.</response>
    /// <returns></returns>
    [HttpPut("{Id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid Id, [FromBody] UpdateUserCommand command)
    {
        _logger.LogInformation("Received request to update user with Id: {UserId}.", Id);

        if (Id != command.Id) 
        {
            _logger.LogWarning("Mismatched user Ids in URL and request body. URL Id: {UrlId}, Body Id: {BodyId}.", Id, command.Id);
            return BadRequest("User Id in URL does not match Id in request body");
        }

        try
        {
            await _mediator.Send(command);
            _logger.LogInformation("Successfully updated user with Id: {UserId}.", Id);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("was not found"))
        {
            _logger.LogWarning(ex, "User with Id: {UserId} not found.", Id);
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Updates the role of an existing user by their ID.
    /// </summary>
    /// <param name="command">
    /// The <see cref="UpdateUserRoleCommand"/> containing the user's ID and the new role to assign.
    /// </param>
    /// <response code="200">User role successfully updated.</response>
    /// <response code="400">If the request is invalid or an error occurs.</response>
    /// <response code="404">If the user is not found.</response>
    /// <returns></returns>
    [HttpPost("update-role")]
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleCommand command)
    {
        if (command == null || command.Id == Guid.Empty || string.IsNullOrEmpty(command.NewRole))
        {
            return BadRequest("Invalid request");
        }

        try
        {
            await _mediator.Send(command);
            return Ok(new { Message = "User role updated successfully", UserId = command.Id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
