namespace Lib.Tests;

using FluentValidation;
using UseValidatorExtension.FluentValidation;

public class BaseValidatorTests
{
    private record Person(string Name, int Age);

    private class PersonValidator : BaseValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            this.RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(18)
                .WithMessage("Age must be 18+");
        }
    }

    [Fact]
    public void ValidatePayload_ReturnsValid_WhenAllRulesPass()
    {
        var validator = new PersonValidator();
        var payload = new Person("Alice", 30);

        var result = validator.ValidatePayload(payload);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ValidatePayload_ReturnsErrors_WhenRulesFail()
    {
        var validator = new PersonValidator();
        var payload = new Person("", 12);

        var result = validator.ValidatePayload(payload);

        Assert.False(result.IsValid);
        Assert.Contains("Name is required", result.Errors);
        Assert.Contains("Age must be 18+", result.Errors);
    }
}
