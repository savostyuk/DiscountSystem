using DiscountSystem.Application.Categories.Commands;
using DiscountSystem.Application.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscountSystem.Application.Categories.Validators;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CategoryName)
            .NotEmpty()
            .WithMessage("CategoryName is required.")
            .MaximumLength(30)
            .WithMessage("CategoryName cannot exceed 30 characters.")
            .MustAsync(BeUniqueName)
            .WithMessage("Category with this name already exsists.");
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _context.Categories.AnyAsync(v => v.CategoryName == name, cancellationToken);
    }
}
