using DiscountSystem.Application.Users.Commands;
using DiscountSystem.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users.
    /// </summary>
    /// <response code="204">Returns the list of users.</response>
    /// <response code="404">If no users are found.</response>
    /// <returns>A list of all users.</returns>
    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetAllUsers() 
    {
        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query);

        if (users is null or []) 
        {
            return NotFound("No user found!");
        }

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
        var userId = await _mediator.Send(command);

        if (userId == Guid.Empty) 
        {
            return BadRequest("An error occured!");
        }

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
        var command = new DeleteUserCommand(Id);
        await _mediator.Send(command);

        return NoContent();
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
        if (Id != command.Id) 
        {
            return BadRequest("User Id in URL does not match Id in request body");
        }

        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("was not found"))
        { 
            return NotFound(ex.Message);
        }
        catch (Exception exc)
        {
            return StatusCode(500, $"Error: {exc.Message}");
        }
    }
}
