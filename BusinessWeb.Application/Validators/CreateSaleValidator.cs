using BusinessWeb.Application.DTOs.Sales;
using BusinessWeb.Domain.Enums;
using FluentValidation;

namespace BusinessWeb.Application.Validators;

public class CreateSaleValidator : AbstractValidator<CreateSaleDto>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.StoreId).NotEmpty();
        RuleFor(x => x.Lines).NotEmpty();

        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.Quantity).GreaterThan(0);
            line.RuleFor(l => l.UnitPrice).GreaterThan(0);
        });

        RuleFor(x => x).Custom((dto, ctx) =>
        {
            if (dto.PaymentType == PaymentType.Debt && dto.PaidAmount != 0)
                ctx.AddFailure("PaidAmount", "Debt bo'lsa PaidAmount 0 bo'lishi shart.");

            if (dto.PaymentType == PaymentType.Partial && dto.PaidAmount <= 0)
                ctx.AddFailure("PaidAmount", "Partial bo'lsa PaidAmount > 0 bo'lishi shart.");

            // Cash bo'lsa to'liq tekshiruvni SaleService ichida total hisoblangandan keyin qilamiz
        });
    }
}
