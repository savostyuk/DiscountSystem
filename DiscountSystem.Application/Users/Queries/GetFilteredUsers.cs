using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DiscountSystem.Application.Users.Queries;

public record GetFilteredUsersQuery : IRequest<List<UserDTO>>
{
    public UserFilter? Filter { get; set; }
}

public class GetFilteredUsersQueryHandler : IRequestHandler<GetFilteredUsersQuery, List<UserDTO>>
{
    private IApplicationDbContext _context;
    public GetFilteredUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDTO>> Handle(GetFilteredUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsNoTracking();

        if (request.Filter != null) 
        { 
            if(!string.IsNullOrWhiteSpace(request.Filter.Email))
            {
                query = query.Where(u => u.Email.Contains(request.Filter.Email));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter.FirstName))
            {
                query = query.Where(u => u.FirstName.Contains(request.Filter.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter.LastName))
            {
                query = query.Where(u => u.LastName.Contains(request.Filter.LastName));
            }
        }

        var users = await query.Select(u => new UserDTO
        {
            Id = u.Id,
            Email = u.Email,
            FullName = $"{u.FirstName} {u.LastName}"
        }).ToListAsync(cancellationToken);

        return users;
    }
}
