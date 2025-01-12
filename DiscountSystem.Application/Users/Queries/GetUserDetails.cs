using DiscountSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DiscountSystem.Application.Users.Queries;

public record GetUserDetailsQuery : IRequest<UserDTO>
{
    public Guid Id { get; set; }
}

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDTO>
{
    private readonly IApplicationDbContext _context;
    public GetUserDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDTO> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var userDetails = await _context.Users
            .Where(u => u.Id == request.Id)
            .Select(u => new UserDTO
            {
                Id = request.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Email = u.Email
            }).FirstOrDefaultAsync(cancellationToken);

        if (userDetails == null)
        {
            throw new Exception($"USer with Id {request.Id} was not found");
        }

        return userDetails;
    }
}
