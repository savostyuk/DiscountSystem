using DiscountSystem.Application.Common;
using DiscountSystem.Application.Discounts.Commands;
using FluentValidation;

namespace DiscountSystem.Application.Discounts.Validators;

public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{

    private readonly IApplicationDbContext _context;

    public CreateDiscountCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(d => d.DiscountName)
            .NotEmpty().WithMessage("Discount name is required");

        RuleFor(d => d.Condition)
            .NotEmpty().WithMessage("Condition is required");

        RuleFor(d => d.Promocode)
            .NotEmpty().WithMessage("Promocode is required");

        RuleFor(d => d.StartDate)
            .LessThan(d => d.EndDate)
            .WithMessage("StartDate must be earlier than EndDate")
            .NotEmpty()
            .WithMessage("Start date is required.");

        RuleFor(d => d.EndDate)
            .GreaterThan(d => d.StartDate)
            .WithMessage("EndDate must be later than StartDate");

        RuleFor(d => d.VendorId)
            .NotEqual(Guid.Empty).WithMessage("Vendor Id is required");

        RuleFor(d => d.CategoryId).Must(id => id == null || id !=Guid.Empty)
            .WithMessage("Valid CategoryId is required");
    }
}
