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

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserCommand command)
    {
        try
        {
            var userId = await _mediator.Send(command);

            if (userId == Guid.Empty) 
            {
                return BadRequest("An error occured!");
            }

            return Ok(userId);
        }
        catch (Exception ex) 
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpDelete("{Id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid Id)
    {
        var command = new DeleteUserCommand(Id);
        await _mediator.Send(command);

        return NoContent();
    }
}
