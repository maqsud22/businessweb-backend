using BusinessWeb.Application.DTOs.Stores;
using FluentValidation;

namespace BusinessWeb.Application.Validators.Stores;

public class StoreUpdateValidator : AbstractValidator<StoreUpdateDto>
{
    public StoreUpdateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(30);
    }
}
