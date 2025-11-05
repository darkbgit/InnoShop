using FluentValidation;
using ProductManagement.Core.DTOs;

namespace ProductManagement.Core.Validation;

internal class ProductForCreateDtoValidator : AbstractValidator<ProductForCreateDto>
{
    public ProductForCreateDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p)
            .NotNull();

        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(120)
            .WithMessage("Invalid Name");

        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Invalid Description");

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");

        RuleFor(p => p)
            .Must(p => p.SalePrice >= p.Price)
            .When(p => p.IsOnSale)
            .WithMessage("SalePrice must be lower then Price.");
    }
}
