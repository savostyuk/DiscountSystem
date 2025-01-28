using DiscountSystem.Application.Favorites.Command;
using DiscountSystem.Application.Favorites.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscountSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FavoritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> AddToFavorite([FromBody] AddToFavoritesCommand command)
        {
            var favorite = await _mediator.Send(command);

            return Ok(favorite);
        }

        [HttpDelete("{discountId:guid}")]
        public async Task<ActionResult> DeleteFromFavorits(Guid discountId)
        {
            var command = new DeleteFromFavoritesCommand(discountId);

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

        [HttpPut("{discountId:guid}")]
        public async Task<IActionResult> UpdateFavoriteNote(Guid discountId, [FromBody] UpdateFavoriteCommand command)
        {
            if (discountId != command.DiscountId)
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

        [HttpGet]
        public async Task<IActionResult> GetFavoritesByUserId()
        {
            var query = new GetFavoritesByUserIdQuery();
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
