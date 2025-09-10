namespace InstallPackageTest;

using FluentValidation;
using UseValidatorExtension.FluentValidation;

public class FakeValidator : BaseValidator<FakeBaseRequest>
{
    public FakeValidator()
    {
        this.RuleFor(x => x.Name).NotEqual("bad").WithMessage("Name cannot be 'bad'");
    }
}
