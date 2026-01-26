using API.Public.DTOs.Auth;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.Auth;

public class AuthenticationValidator : BaseValidator<AuthenticateDTO>
{
    public AuthenticationValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(m => m.Password)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(6, 30).WithMessage("INVALID_LENGTH");
    }
}
