using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DiscountSystem.Application.Favorites.Command;

public record DeleteFromFavoritesCommand(Guid discountId) : IRequest
{
}

public class DeleteFromFavoritesCommandHandler : IRequestHandler<DeleteFromFavoritesCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteFromFavoritesCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(DeleteFromFavoritesCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userGuid = Guid.Parse(userId);

        var entity = await _context.Favorites.FindAsync([userGuid, request.discountId], cancellationToken);

        _context.Favorites.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
