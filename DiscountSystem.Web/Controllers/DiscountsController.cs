using DiscountSystem.Application.Discounts.Queries;
using DiscountSystem.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountsController(IMediator mediator)
    {
        _mediator = mediator;
    }

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
}
