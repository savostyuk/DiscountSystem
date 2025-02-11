﻿using DiscountSystem.Application.Common;
using MediatR;

namespace DiscountSystem.Application.Users.Commands;

public record UpdateUserCommand : IRequest
{
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Location { get; init; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            throw new Exception($"Entity with Id = {request.Id} was not found");
        }

         entity.FirstName = request.FirstName;
         entity.LastName = request.LastName;
         entity.Location = request.Location;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
