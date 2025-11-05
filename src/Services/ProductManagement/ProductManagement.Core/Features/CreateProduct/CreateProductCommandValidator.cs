using FluentValidation;

namespace ProductManagement.Core.Features.CreateProduct;

internal class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
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

        RuleFor(p => p.SalePrice)
            .GreaterThan(0)
            .When(p => p.IsOnSale)
            .WithMessage("SalePrice must be greater than zero.");

        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .WithMessage("Invalid category.");

        RuleFor(p => p.CreatedBy)
            .NotEmpty()
            .WithMessage("Invalid user.");
    }

}
