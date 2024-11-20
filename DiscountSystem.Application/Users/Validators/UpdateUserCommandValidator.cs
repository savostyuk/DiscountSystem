using DiscountSystem.Application.Common;
using DiscountSystem.Application.Users.Commands;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Users.Validators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;
    public UpdateUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(p => p.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required.")
            .MaximumLength(100)
            .WithMessage("First Name cannot exceed 100 characters.");

        RuleFor(p => p.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required.")
            .MaximumLength(100)
            .WithMessage("Last Name cannot exceed 100 characters.");

        RuleFor(p => p.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Please enter a valid email address.")
            .MustAsync(BeUniqueEmail)
            .WithMessage("User with this email already exsists.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }
}
