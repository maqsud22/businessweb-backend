using BusinessWeb.Application.DTOs.Debts;
using FluentValidation;

namespace BusinessWeb.Application.Validators.Debts;

public class DebtPayValidator : AbstractValidator<DebtPayRequestDto>
{
    public DebtPayValidator()
    {
        RuleFor(x => x.DebtId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
