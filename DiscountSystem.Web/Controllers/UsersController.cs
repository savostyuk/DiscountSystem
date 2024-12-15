using DiscountSystem.Application.Users.Commands;
using DiscountSystem.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    /// Create a new user.
    /// </summary>
    /// <param name="command">The command to create a user.</param>
    /// <response code="204">Returns the Id of the newly created user.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <returns>The Id of the created user.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserCommand command)
    {
        _logger.LogInformation("Received request to create a new user.");
        var userId = await _mediator.Send(command);

        if (userId == Guid.Empty) 
        {
            _logger.LogWarning("User creation failed.");
            return BadRequest("An error occured!");
        }

        _logger.LogInformation("Successfully created user with Id: {UserId}.", userId);
        return Ok(userId);
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
}
