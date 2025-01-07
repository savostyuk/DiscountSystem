using DiscountSystem.Application.Tags.Commands;
using DiscountSystem.Application.Tags.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]

public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;
    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new tag.
    /// </summary>
    /// <param name="command">The command to create a tag.</param>
    /// <response code="204">Returns the Id of the newly created tag.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <returns>The Id of the created tag.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTag([FromBody] CreateTagCommand command)
    {
        var tagId = await _mediator.Send(command);

        if (tagId == Guid.Empty)
        {
            return BadRequest("An error occured!");
        }

        return Ok(tagId);
    }

    /// <summary>
    /// Delete a tag by Id.
    /// </summary>
    /// <param name="Id">The id of the tag.</param>
    /// <response code="204">Tag successfully deleted.</response>
    /// <response code="404">If the tag is not found.</response>
    /// <returns></returns>
    [HttpDelete("{Id:guid}")]
    public async Task<ActionResult> DeleteTag(Guid Id)
    {
        var command = new DeleteTagCommand(Id);

        try
        {
            await _mediator.Send(command);

            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }

    }

    /// <summary>
    /// Update an existing tag by Id.
    /// </summary>
    /// <param name="Id">The id of the tag.</param>
    /// <response code="204">Tag successfully updated.</response>
    /// <response code="404">If the tag is not found.</response>
    /// <response code="400">If the tag request is invalid.</response>
    /// <returns></returns>
    [HttpPut("{Id:guid}")]
    public async Task<IActionResult> UpdateTag(Guid Id, [FromBody] UpdateTagCommand command)
    {
        if (Id != command.Id)
        {
            return BadRequest("Tag Id in URL does not match Id in request body");
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
    }

    /// <summary>
    /// Get a tag by its unique identifier.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <response code="200">Tag successfully retrieved.</response>
    /// <response code="404">If the tag is not found.</response>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("{tagId:guid}")]
    public async Task<IActionResult> GetTagById(Guid tagId)
    {
        var query = new GetTagByIdQuery { Id = tagId };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
}
