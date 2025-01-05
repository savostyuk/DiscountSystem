using DiscountSystem.Application.Common;
using DiscountSystem.Application.Common.Extensions;
using DiscountSystem.Application.Common.Models;
using MediatR;

namespace DiscountSystem.Application.Users.Queries;

public record GetPaginatedUsersQuery : IRequest<PaginatedList<UserDTO>>
{
    public int PageSize { get; init; } = 10;
    public int PageNumber { get; init; } = 1;
}

public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, PaginatedList<UserDTO>>
{
    private readonly IApplicationDbContext _context;
    public GetPaginatedUsersQueryHandler (IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<UserDTO>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Email = u.Email
            }).ToPaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
