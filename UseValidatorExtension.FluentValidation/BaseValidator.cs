namespace UseValidatorExtension.FluentValidation;

using global::FluentValidation;
using UseValidator;

public class BaseValidator<T> : AbstractValidator<T>, global::UseValidator.IValidator<T>
{
    public ValidationResult ValidatePayload(T payload)
    {
        var result = this.Validate(payload);

        return new ValidationResult
        {
            IsValid = result.IsValid, Errors = result.Errors.Select(e => e.ErrorMessage).ToList()
        };
    }
}
