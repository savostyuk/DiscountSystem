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

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateVendor([FromBody] CreateVendorCommand command)
    {
        try
        {
            var vendorId = await _mediator.Send(command);

            if (vendorId == Guid.Empty)
            {
                return BadRequest("An error occured!");
            }

            return Ok(vendorId);

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpDelete("{Id:guid}")] 
    public async Task<ActionResult> DeleteVendor (Guid Id)
    {
        var command = new DeleteVendorCommand(Id);
        await _mediator.Send(command);

        return NoContent();
    }
}
