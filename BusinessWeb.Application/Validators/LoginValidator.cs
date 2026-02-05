using BusinessWeb.Application.DTOs.Auth;
using FluentValidation;

namespace BusinessWeb.Application.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(64);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(128);
    }
}
