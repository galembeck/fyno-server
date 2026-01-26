using Domain.Data.Models.Util;
using Domain.Enumerators;
using FluentValidation.Results;

namespace Domain.Exceptions;

public class ValidationException : ApplicationException
{
    public ValidationErrorModel Error { get; }

    public ValidationException(System.Exception innerExc) : base(innerExc.GetType().Name, innerExc)
    {
        Error = new ValidationErrorModel();
    }

    public ValidationException(string innerExc) : base(innerExc)
    {
        Error = new ValidationErrorModel();
    }

    public ValidationException(ValidationErrorMessage innerExc) : base(innerExc.ToString())
    {
        Error = new ValidationErrorModel();
    }

    public ValidationException(ValidationErrorMessage innerExc, IList<ValidationFailure> errors) : base(innerExc.ToString())
    {
        ValidationFailure? error = errors.FirstOrDefault();

        Error = new ValidationErrorModel()
        {
            Message = error?.ErrorMessage ?? string.Empty,
            Property = error?.PropertyName ?? string.Empty,
        };
    }
}
