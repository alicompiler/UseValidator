namespace Lib.Tests;

using UseValidator;

public class StringPassValidator : IValidator<string>
{
    public ValidationResult ValidatePayload(string payload)
    {
        return new ValidationResult
        {
            IsValid = true, Errors = []
        };
    }
}

public class StringFailValidator : IValidator<string>
{
    public ValidationResult ValidatePayload(string payload)
    {
        return new ValidationResult
        {
            IsValid = false, Errors = ["bad"]
        };
    }
}

public class StringMultiErrorValidator : IValidator<string>
{
    public ValidationResult ValidatePayload(string payload)
    {
        return new ValidationResult
        {
            IsValid = false, Errors = ["bad1", "bad2"]
        };
    }
}
