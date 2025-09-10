namespace InstallPackageTest;

using UseValidator;

public class FakeValidator : IValidator<FakeBaseRequest>
{
    public ValidationResult ValidatePayload(FakeBaseRequest payload)
    {
        if (payload.Name == "bad")
        {
            return new ValidationResult
            {
                IsValid = false, Errors = ["Name cannot be 'bad'"]
            };
        }

        return new ValidationResult
        {
            IsValid = true, Errors = []
        };
    }
}
