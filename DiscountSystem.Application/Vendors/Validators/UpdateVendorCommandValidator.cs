using DiscountSystem.Application.Common;
using DiscountSystem.Application.Vendors.Commands;
using FluentValidation;

namespace DiscountSystem.Application.Vendors.Validators;

public class UpdateVendorCommandValidator : AbstractValidator<UpdateVendorCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateVendorCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(p => p.VendorName)
            .NotEmpty()
            .WithMessage("VendorName is required.")
            .MaximumLength(100)
            .WithMessage("VendorName cannot exceed 100 characters.");

        RuleFor(p => p.WorkingHours)
            .MaximumLength(50)
            .WithMessage("WorkingHours cannot exceed 50 characters.");

        RuleFor(p => p.Website)
            .NotEmpty()
            .WithMessage("Website URL is required.")
            .Matches(@"^(https?://)?([a-z0-9-]+\.)+[a-z]{2,6}(/[\w\.-]*)*/?$")
            .WithMessage("Please enter a valid URL.");

        RuleFor(p => p.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");

        RuleFor(p => p.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\d+$")
            .WithMessage("Phone number must contain only numbers.");

        RuleFor(p => p.Address)
            .NotEmpty()
            .WithMessage("Address number is required.")
            .Length(5, 250)
            .WithMessage("Address must be between 5 and 250 characters.");
    }
}
