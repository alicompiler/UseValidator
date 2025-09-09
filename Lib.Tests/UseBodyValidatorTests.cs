namespace Lib.Tests;

using Helper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using UseValidator;

public class UseBodyValidatorTests
{
    [Fact]
    public async Task UseBodyValidator_CallsNext_WhenValid()
    {
        var ctx = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>
        {
            {
                "body", "xyz"
            }
        }, "body", BindingSource.Body);

        var attr = new UseBodyValidator
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
    public async Task UseBodyValidator_SetBadRequest_WhenInvalid()
    {
        var ctx = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>
        {
            {
                "body", "xyz"
            }
        }, "body", BindingSource.Body);

        var attr = new UseBodyValidator
        {
            Validator = typeof(StringFailValidator)
        };

        var isNextCalled = new Flag();
        var next = ActionExecutionDelegateHelper.MakeNext(isNextCalled);

        await attr.OnActionExecutionAsync(ctx, next);

        Assert.False(isNextCalled.Value);
        Assert.NotNull(ctx.Result);
    }
}
