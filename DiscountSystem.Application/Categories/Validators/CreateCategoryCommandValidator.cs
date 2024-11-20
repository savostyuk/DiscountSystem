using DiscountSystem.Application.Categories.Commands;
using DiscountSystem.Application.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Categories.Validators;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CategoryName)
            .NotEmpty()
            .WithMessage("CategoryName is required.")
            .MaximumLength(30)
            .WithMessage("CategoryName cannot exceed 30 characters.")
            .MustAsync(BeUniqueName)
            .WithMessage("Category name must be unique.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _context.Categories.AnyAsync(v => v.CategoryName == name, cancellationToken);
    }
}
