using DiscountSystem.Application.Common.Models;
using DiscountSystem.Application.Discounts.Commands;
using DiscountSystem.Application.Discounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Moderator")]
public class DiscountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all discounts.
    /// </summary>
    /// <response code="204">Returns the list of discounts.</response>
    /// <response code="404">If no discounts are found.</response>
    /// <returns>A list of all discounts.</returns>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<List<DiscountDTO>>> GetAllDiscounts()
    {
        var query = new GetAllDiscountsQuery();
        var discounts = await _mediator.Send(query);

        if (discounts is null or [])
        {
            return NotFound("No discount found!");
        }

        return Ok(discounts);
    }

    /// <summary>
    /// Get discounts with pagination.
    /// </summary>
    /// <response code="204">Returns the page of discounts.</response>
    /// <response code="404">If no discounts are found.</response>
    /// <returns>A page of discounts.</returns>
    [AllowAnonymous]
    [HttpGet("paginated")]
    public async Task<PaginatedList<DiscountDTO>> GetDiscountsWithPagination([FromQuery] GetPaginatedDiscountsQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Get details of a specific discount by Id.
    /// </summary>
    /// <param name="Id">The id of the discount.</param>
    /// <response code="204">Returns the discount.</response>
    /// <response code="404">If the discount is not found.</response>
    /// <returns>The details of the specified discount.</returns>
    [AllowAnonymous]
    [HttpGet("{discountId:guid}")]
    public async Task<IActionResult> GetDiscountDetails(Guid discountId)
    {
        var query = new GetDiscountDetailsQuery { Id = discountId };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Create a new discount.
    /// </summary>
    /// <param name="command">The command to create a discount.</param>
    /// <response code="204">Returns the Id of the newly created discount.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <returns>The Id of the created discount.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateVendor([FromBody] CreateDiscountCommand command)
    {
        var discountId = await _mediator.Send(command);

        if (discountId == Guid.Empty)
        {
            return BadRequest("An error occured!");
        }

        return Ok(discountId);
    }

    /// <summary>
    /// Update an existing discount by Id.
    /// </summary>
    /// <param name="Id">The id of the discount.</param>
    /// <response code="204">Discount successfully updated.</response>
    /// <response code="404">If the discount is not found.</response>
    /// <response code="400">If the discount request is invalid.</response>
    /// <returns></returns>
    [HttpPut("{Id:guid}")]
    public async Task<IActionResult> UpdateDiscount(Guid Id, [FromBody] UpdateDiscountCommand command)
    {
        if (Id != command.Id)
        {
            return BadRequest("Discount Id in URL does not match Id in request body");
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
    /// Delete a discount by Id.
    /// </summary>
    /// <param name="Id">The id of the discount.</param>
    /// <response code="204">Discount successfully deleted.</response>
    /// <response code="404">If the discount is not found.</response>
    /// <returns></returns>
    [HttpDelete("{Id:guid}")]
    public async Task<ActionResult> DeleteDiscount(Guid Id)
    {
        var command = new DeleteDiscountCommand(Id);

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
}
