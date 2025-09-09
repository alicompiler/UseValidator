namespace Lib.Tests;

using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UseValidator;

public class UseValidatorBaseTests
{
    private class NullReturningValidator : IValidator<string>
    {
        public ValidationResult ValidatePayload(string payload) => null!;
    }

    [Fact]
    public async Task Base_SetsBadRequest_WithEmptyErrors_WhenValidatorReturnsNull()
    {
        var ctx = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>
        {
            {"query", "hello"}
        }, "query", BindingSource.Query);

        var attr = new UseQueryValidator
        {
            Validator = typeof(NullReturningValidator)
        };

        var isNextCalled = new Flag();
        var next = ActionExecutionDelegateHelper.MakeNext(isNextCalled);

        await attr.OnActionExecutionAsync(ctx, next);

        Assert.False(isNextCalled.Value);
        var br = Assert.IsType<BadRequestObjectResult>(ctx.Result);
        var errors = Assert.IsType<List<string>>(br.Value);
        Assert.Empty(errors);
    }
}
