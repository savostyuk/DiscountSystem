using DiscountSystem.Application.Users.Commands;
using FluentValidation;

namespace DiscountSystem.Application.Users.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
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
            .WithMessage("Please enter a valid email address.");

        RuleFor(p => p.Role)
            .NotEmpty()
            .WithMessage("Role is required.");
    }
}
