namespace Lib.Tests;

using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UseValidator;

public class UseQueryValidatorTests
{
    [Fact]
    public async Task UseQueryValidator_CallsNext_WhenValid()
    {
        var ctx = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>
        {
            {
                "query", "hello"
            }
        }, "query", BindingSource.Query);

        var attr = new UseQueryValidator
        {
            Validator = typeof(StringPassValidator)
        };

        var isNextCalled = new Flag();
        var next = ActionExecutionDelegateHelper.MakeNext(isNextCalled);

        await attr.OnActionExecutionAsync(ctx, next);

        Assert.True(isNextCalled.Value);
        Assert.Null(ctx.Result);
    }

    [Fact]
    public async Task UseQueryValidator_SetsBadRequest_WhenInvalid()
    {
        var ctx = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>
        {
            {
                "query", "hello"
            }
        }, "query", BindingSource.Query);

        var attr = new UseQueryValidator
        {
            Validator = typeof(StringFailValidator)
        };

        var isNextCalled = new Flag();
        var next = ActionExecutionDelegateHelper.MakeNext(isNextCalled);

        await attr.OnActionExecutionAsync(ctx, next);

        Assert.False(isNextCalled.Value);
        Assert.IsType<BadRequestObjectResult>(ctx.Result);
        var br = (BadRequestObjectResult)ctx.Result!;
        var errors = Assert.IsType<List<string>>(br.Value);
        Assert.Single(errors);
        Assert.Equal("bad", errors[0]);
    }
}
