using API.Public.DTOs.Client;
using API.Public.Validators._Base;
using Domain.Utils;
using FluentValidation;

namespace API.Public.Validators.Client;

public class ClientCreationValidator : BaseValidator<PrivateClientDTO>
{
    public ClientCreationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .EmailAddress().WithMessage("INVALID_EMAIL_FORMAT");

        RuleFor(x => x.PrimaryDocument)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Must(StringUtil.IsValidCPForCNPJ).WithMessage("INVALID_DOCUMENT");

        RuleFor(x => x.Cellphone)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Must(StringUtil.IsValidCellphone).WithMessage("INVALID_LENGTH");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");
    }
}
