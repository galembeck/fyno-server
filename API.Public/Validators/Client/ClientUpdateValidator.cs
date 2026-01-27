using API.Public.DTOs.Client.Payloads;
using API.Public.Validators._Base;
using Domain.Utils;
using FluentValidation;

namespace API.Public.Validators.Client;

public class ClientUpdateValidator : BaseValidator<UpdateClientDTO>
{
    public ClientUpdateValidator()
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
        });

        When(x => x.Email != null, () =>
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
                .EmailAddress().WithMessage("INVALID_EMAIL_FORMAT");
        });

        When(x => x.PrimaryDocument != null, () =>
        {
            RuleFor(c => c.PrimaryDocument)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
                .Must(StringUtil.IsValidCPForCNPJ).WithMessage("INVALID_DOCUMENT");
        });

        When(x => x.Cellphone != null, () =>
        {
            RuleFor(c => c.Cellphone)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
                .Must(cellphone => cellphone != null && 
                    StringUtil.IsValidCellphone(cellphone)).WithMessage("INVALID_LENGTH");
        });
        When(x => x.Address != null, () =>
        {
            RuleFor(c => c.Address)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
        });
    }
}
