using DiscountSystem.Application.Common;
using DiscountSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DiscountSystem.Application.Favorites.Command;

public record AddToFavoritesCommand : IRequest<Guid>
{
    public Guid DiscountId { get; set; }
}

public class AddToFavoritesCommandHandler : IRequestHandler<AddToFavoritesCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddToFavoritesCommandHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
    {
        //GetCurrentUserService
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userGuid = Guid.Parse(userId);

        var entity = new Favorite
        {
            UserId = userGuid,
            DiscountId = request.DiscountId,
        } ?? throw new Exception($"Favorites cannot be addedd");

        _context.Favorites.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.DiscountId;
    }
}
