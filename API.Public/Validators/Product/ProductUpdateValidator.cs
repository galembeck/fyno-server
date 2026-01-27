using API.Public.DTOs.Product.Payloads;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.Product;

public class ProductUpdateValidator : BaseValidator<UpdateProductDTO>
{
    public ProductUpdateValidator()
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
        });

        When(x => x.Description != null, () =>
        {
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("CANNOT_BE_EMPTY");
        });
        When(x => x.Price != null, () =>
        {
            RuleFor(c => c.Price)
                .GreaterThanOrEqualTo(1.00m).WithMessage("GREATER_OR_EQUALS_ONE");
        });
    }
}
