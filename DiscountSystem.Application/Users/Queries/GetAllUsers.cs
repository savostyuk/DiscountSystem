using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Users.Queries;

public record GetAllUsers : IRequest<List<UserDTO>>
{
}

public class GetAllUsersHandler : IRequestHandler<GetAllUsers, List<UserDTO>>
{
    private readonly IApplicationDbContext _context;
    public GetAllUsersHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDTO>> Handle(GetAllUsers request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Select(u => new UserDTO
            {
                Id = u.Id,
                Email = u.Email,
                FullName = $"{u.FirstName} {u.LastName}"
            }).ToListAsync(cancellationToken);
    }
}