using BusinessWeb.Application.DTOs.ProductPackages;
using FluentValidation;

namespace BusinessWeb.Application.Validators.ProductPackages;

public class ProductPackageCreateValidator : AbstractValidator<ProductPackageCreateDto>
{
    public ProductPackageCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Multiplier).GreaterThan(0);
        RuleFor(x => x.ProductId).NotEmpty();
    }
}
