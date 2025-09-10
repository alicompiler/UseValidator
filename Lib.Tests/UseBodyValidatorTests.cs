namespace Lib.Tests;

using Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
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
                "body", "hello"
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
    public async Task UseBodyValidator_SetsBadRequest_WhenInvalid()
    {
        var ctx = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>
        {
            {
                "body", "hello"
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
        Assert.IsType<BadRequestObjectResult>(ctx.Result);
        var br = (BadRequestObjectResult)ctx.Result!;
        var errors = Assert.IsType<List<string>>(br.Value);
        Assert.Single(errors);
        Assert.Equal("bad", errors[0]);
    }
    [Fact]
    public async Task UseBodyValidator_CallsNext_WhenArgumentMissing()
    {
        // build context with one FromBody parameter but without providing ActionArguments
        var context = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>(), "body", BindingSource.Body);
        var actionDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "Test",
            ActionName = "TestAction",
            Parameters = new List<ParameterDescriptor>
            {
                new()
                {
                    Name = "body",
                    BindingInfo = new BindingInfo
                    {
                        BindingSource = BindingSource.Body
                    }
                }
            }
        };

        context.ActionDescriptor = actionDescriptor;

        var attr = new UseBodyValidator
        {
            Validator = typeof(StringPassValidator)
        };

        var isNextCalled = new Flag();
        var next = ActionExecutionDelegateHelper.MakeNext(isNextCalled);

        await Assert.ThrowsAsync<InvalidOperationException>(() => attr.OnActionExecutionAsync(context, next));
    }

    [Fact]
    public async Task UseBodyValidator_Throws_WhenZeroOrMultipleFromBodyParams()
    {
        var contextZero = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>
        {
            {
                "a", 1
            },
            {
                "b", 2
            }
        }, "", BindingSource.Custom);

        var attr = new UseBodyValidator
        {
            Validator = typeof(StringPassValidator)
        };

        var next = ActionExecutionDelegateHelper.MakeNext(new Flag());
        await Assert.ThrowsAsync<InvalidOperationException>(() => attr.OnActionExecutionAsync(contextZero, next));

        // multiple: build a context with two FromBody parameters
        var contextMulti = ActionExecutionDelegateHelper.BuildContext(new Dictionary<string, object?>(), "", BindingSource.Custom);
        var actionDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "Test",
            ActionName = "TestAction",
            Parameters = new List<ParameterDescriptor>
            {
                new()
                {
                    Name = "q1",
                    BindingInfo = new BindingInfo
                    {
                        BindingSource = BindingSource.Body
                    }
                },
                new()
                {
                    Name = "q2",
                    BindingInfo = new BindingInfo
                    {
                        BindingSource = BindingSource.Body
                    }
                }
            }
        };

        contextMulti.ActionDescriptor = actionDescriptor;

        await Assert.ThrowsAsync<InvalidOperationException>(() => attr.OnActionExecutionAsync(contextMulti, next));
    }
}
