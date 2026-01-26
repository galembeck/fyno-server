using API.Public.DTOs.User.Payloads;
using API.Public.Validators._Base;
using Domain.Utils;
using FluentValidation;

namespace API.Public.Validators.User;

public class UserUpdateValidator : BaseValidator<UpdateUserDTO>
{
    public UserUpdateValidator()
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
        });

        When(x => x.Email != null, () =>
        {
            RuleFor(c => c.Email)
                .EmailAddress().WithMessage("INVALID_EMAIL")
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
        });

        When(x => x.Cellphone != null, () =>
        {
            RuleFor(c => c.Cellphone)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
                .Length(10, 16).WithMessage("INVALID_LENGTH")
                .Must(StringUtil.IsValidCellphone).WithMessage("INVALID_CELLPHONE");
        });

        When(x => x.SupportCellphone != null, () =>
        {
            RuleFor(c => c.SupportCellphone)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
                .Length(10, 16).WithMessage("INVALID_LENGTH")
                .Must(StringUtil.IsValidCellphone).WithMessage("INVALID_CELLPHONE");
        });

        When(x => x.CompanyInformation != null, () =>
        {
            When(x => (int)x.CompanyInformation.MonthlyRevenue != 0, () =>
            {
                RuleFor(c => c.CompanyInformation.MonthlyRevenue)
                    .IsInEnum()
                    .WithMessage("INVALID_ENUM");
            });

            When(x => (int)x.CompanyInformation.BusinessSegment != 0, () =>
            {
                RuleFor(c => c.CompanyInformation.BusinessSegment)
                    .IsInEnum()
                    .WithMessage("INVALID_ENUM");
            });
        });

        When(x => x.AddressInformation != null, () =>
        {
            When(x => x.AddressInformation.Address != null, () =>
            {
                RuleFor(c => c.AddressInformation.Address)
                    .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
            });

            When(x => x.AddressInformation.Number != null, () =>
            {
                RuleFor(c => c.AddressInformation.Number)
                    .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
            });

            When(x => x.AddressInformation.Zipcode != null, () =>
            {
                RuleFor(c => c.AddressInformation.Zipcode)
                    .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
            });

            When(x => x.AddressInformation.Neighborhood != null, () =>
            {
                RuleFor(c => c.AddressInformation.Neighborhood)
                    .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
            });

            When(x => !string.IsNullOrEmpty(x.AddressInformation.State), () =>
            {
                RuleFor(c => c.AddressInformation.State)
                    .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
            });

            When(x => !string.IsNullOrEmpty(x.AddressInformation.City), () =>
            {
                RuleFor(c => c.AddressInformation.City)
                    .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
            });
        });
    }
}