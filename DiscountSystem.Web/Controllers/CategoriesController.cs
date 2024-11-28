using DiscountSystem.Application.Categories.Commands;
using DiscountSystem.Application.Categories.Queries;
using DiscountSystem.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all categories.
    /// </summary>
    /// <response code="204">Returns the list of categories.</response>
    /// <response code="404">If no categories are found.</response>
    /// <returns>A list of all categories.</returns>
    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> GetAllCategories()
    {
        var query = new GetAllCategoriesQuery();
        var categories = await _mediator.Send(query);

        if (categories is null or [])
        {
            return NotFound("No category found!");
        }

        return Ok(categories);
    }

    /// <summary>
    /// Create a new category.
    /// </summary>
    /// <param name="command">The command to create a category.</param>
    /// <response code="204">Returns the Id of the newly created category.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <returns>The Id of the created category.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var categoryId = await _mediator.Send(command);

        if (categoryId == Guid.Empty)
        {
            return BadRequest("An error occured!");
        }

        return Ok(categoryId);
    }

    /// <summary>
    /// Delete a category by Id.
    /// </summary>
    /// <param name="Id">The id of the category.</param>
    /// <response code="204">Category successfully deleted.</response>
    /// <response code="404">If the category is not found.</response>
    /// <returns></returns>
    [HttpDelete("{Id:guid}")]
    public async Task<ActionResult> DeleteCategory(Guid Id)
    {
        var command = new DeleteCategoryCommand(Id);

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
    /// Update an existing category by Id.
    /// </summary>
    /// <param name="Id">The id of the category.</param>
    /// <response code="204">Category successfully updated.</response>
    /// <response code="404">If the category is not found.</response>
    /// <response code="400">If the category request is invalid.</response>
    /// <returns></returns>
    [HttpPut("{Id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid Id, [FromBody] UpdateCategoryCommand command)
    {
        if (Id != command.Id)
        {
            return BadRequest("Category Id in URL does not match Id in request body");
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

    [HttpGet("{id}/discounts")] //api/categories/{id}/discounts
    public async Task<IActionResult> GetDiscountsByCategoryId(Guid categoryId)
    {
        var query = new GetDiscountByCategoryIdQuery(categoryId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
