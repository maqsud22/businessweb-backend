using BusinessWeb.Application.DTOs.Sales;
using BusinessWeb.Domain.Enums;
using FluentValidation;

namespace BusinessWeb.Application.Validators.Sales;

public class CreateSaleValidator : AbstractValidator<CreateSaleDto>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.StoreId).NotEmpty();
        RuleFor(x => x.Lines).NotEmpty().WithMessage("Sale lines bo'sh bo'lishi mumkin emas.");

        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId).NotEmpty();
            line.RuleFor(l => l.ProductPackageId).NotEmpty();
            line.RuleFor(l => l.Quantity).GreaterThan(0);
            line.RuleFor(l => l.UnitPrice).GreaterThan(0);
        });

        RuleFor(x => x).Custom((dto, ctx) =>
        {
            // Bu yerda totalni bilmaymiz (DBdan multiplier va checklar kerak)
            // Shuning uchun faqat mantiqiy minimumlarni tekshiramiz.
            if (dto.PaymentType == PaymentType.Debt && dto.PaidAmount != 0)
                ctx.AddFailure(nameof(dto.PaidAmount), "Debt bo'lsa PaidAmount 0 bo'lishi shart.");

            if (dto.PaymentType == PaymentType.Partial && dto.PaidAmount <= 0)
                ctx.AddFailure(nameof(dto.PaidAmount), "Partial bo'lsa PaidAmount > 0 bo'lishi shart.");
        });
    }
}
