using BusinessWeb.Application.DTOs.StockIns;
using FluentValidation;

namespace BusinessWeb.Application.Validators.StockIns;

public class StockInUpdateValidator : AbstractValidator<StockInUpdateDto>
{
    public StockInUpdateValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.ProductPackageId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Date).NotEmpty();
    }
}
