using API.Public.DTOs.Product;
using API.Public.Validators._Base;
using FluentValidation;

namespace API.Public.Validators.Product;

public class ProductCreationValidator : BaseValidator<ProductDTO>
{
    public ProductCreationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("CANNOT_BE_EMPTY")
            .NotNull().WithMessage("CANNOT_BE_NULL");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(1.00m).WithMessage("GREATER_OR_EQUALS_ONE")
            .NotNull().WithMessage("CANNOT_BE_NULL");
    }
}
