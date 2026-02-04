using BusinessWeb.Application.DTOs.Products;
using FluentValidation;

namespace BusinessWeb.Application.Validators.Products;

public class ProductCreateValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(150);
        RuleFor(x => x.DefaultPrice).GreaterThanOrEqualTo(0);
    }
}
