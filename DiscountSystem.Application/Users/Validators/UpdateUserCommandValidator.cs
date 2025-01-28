using DiscountSystem.Application.Common;
using DiscountSystem.Application.Users.Commands;
using FluentValidation;

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

        RuleFor(p => p.Location)
            .NotEmpty()
            .WithMessage("Location is required.")
            .MaximumLength(100)
            .WithMessage("Location cannot exceed 100 characters.");
    }
}
