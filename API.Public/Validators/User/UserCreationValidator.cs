using API.Public.DTOs.User;
using API.Public.Validators._Base;
using Domain.Utils;
using FluentValidation;

namespace API.Public.Validators.User;

public class UserCreationValidator : BaseValidator<PrivateUserDTO>
{
    public UserCreationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .EmailAddress().WithMessage("INVALID_EMAIL_FORMAT");

        RuleFor(x => x.Cellphone)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(10, 16).WithMessage("INVALID_LENGTH")
            .Must(SecurityUtil.IsValidCellphone).WithMessage("INVALID_LENGTH");

        RuleFor(x => x.SupportCellphone)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Length(10, 16).WithMessage("INVALID_LENGTH")
            .Must(SecurityUtil.IsValidCellphone).WithMessage("INVALID_LENGTH");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Must(SecurityUtil.GetPasswordStrength).WithMessage("INVALID_PASSWORD");



        RuleFor(x => x.CompanyInformation.CompanyName)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.CompanyInformation.CompanyDocument)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL")
            .Must(StringUtil.IsValidCNPJ).WithMessage("INVALID_DOCUMENT");

        RuleFor(x => x.CompanyInformation.MonthlyRevenue)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY");

        RuleFor(x => x.CompanyInformation.CompanyDomain)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.CompanyInformation.BusinessSegment)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY");



        RuleFor(x => x.AddressInformation.Address)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.AddressInformation.Number)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.AddressInformation.Neighborhood)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.AddressInformation.Zipcode)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.AddressInformation.State)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.AddressInformation.City)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");
    }
}
