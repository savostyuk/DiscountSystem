using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DiscountSystem.Application.Favorites.Command;

public record UpdateFavoriteCommand : IRequest
{
    public string Note { get; init; }
    public Guid DiscountId { get; set; }
}

public class UpdateFavoriteCommandHandler : IRequestHandler<UpdateFavoriteCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateFavoriteCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(UpdateFavoriteCommand request, CancellationToken cancellationToken)
    {
        //Есть CurrentUserService для получения id текущего пользователя, лучше использовать его, при необходимости расширить
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userGuid = Guid.Parse(userId);

        var entity = await _context.Favorites
            .FindAsync([userGuid, request.DiscountId], cancellationToken);

        if (entity == null)
        {
            throw new KeyNotFoundException("Favorite not found.");
        }

        entity.Note = request.Note;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
