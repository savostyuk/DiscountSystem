using DiscountSystem.Application.Users.Queries;
using DiscountSystem.Application.Vendors.Commands;
using DiscountSystem.Application.Vendors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VendorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all vendors.
    /// </summary>
    /// <response code="204">Returns the list of vendors.</response>
    /// <response code="404">If no vendors are found.</response>
    /// <returns>A list of all vendors.</returns>
    [HttpGet]
    public async Task<ActionResult<List<VendorDTO>>> GetAllVendors()
    {
        var query = new GetAllVendorsQuery();
        var vendors = await _mediator.Send(query);

        if (vendors is null or [])
        {
            return NotFound("No vendor found!");
        }

        return Ok(vendors);
    }

    /// <summary>
    /// Create a new vendor.
    /// </summary>
    /// <param name="command">The command to create a vendor.</param>
    /// <response code="204">Returns the Id of the newly created vendor.</response>
    /// <response code="400">If the request is invalid.</response>
    /// <returns>The Id of the created vendor.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> CreateVendor([FromBody] CreateVendorCommand command)
    {
        var vendorId = await _mediator.Send(command);

        if (vendorId == Guid.Empty)
        {
            return BadRequest("An error occured!");
        }

        return Ok(vendorId);
    }

    /// <summary>
    /// Delete a vendor by Id.
    /// </summary>
    /// <param name="Id">The id of the vendor.</param>
    /// <response code="204">Vendor successfully deleted.</response>
    /// <response code="404">If the vendor is not found.</response>
    /// <returns></returns>
    [HttpDelete("{Id:guid}")]
    public async Task<ActionResult> DeleteVendor(Guid Id)
    {
        var command = new DeleteVendorCommand(Id);

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
    /// Update an existing vendor by Id.
    /// </summary>
    /// <param name="Id">The id of the vendor.</param>
    /// <response code="204">Vendor successfully updated.</response>
    /// <response code="404">If the vendor is not found.</response>
    /// <response code="400">If the vendor request is invalid.</response>
    /// <returns></returns>
    [HttpPut("{Id:guid}")]
    public async Task<IActionResult> UpdateVendor(Guid Id, [FromBody] UpdateVendorCommand command)
    {
        if (Id != command.Id)
        {
            return BadRequest("Vendor Id in URL does not match Id in request body");
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
    /// Get all discounts by Vendor Id.
    /// </summary>
    /// <param name="Id">The id of the vendor.</param>
    /// <response code="204">Returns the list of discounts by Vendor id.</response>
    /// <returns>A list of discounts by Vendor id.</returns>
    [HttpGet("{id}/discounts")]  //api/vendors/{id}/discounts
    public async Task<IActionResult> GetDiscountsByVendorId(Guid vendorId)
    {
        var query = new GetDiscountByVendorIdQuery(vendorId);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
