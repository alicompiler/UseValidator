namespace Lib.Tests.Helper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

public class Flag
{
    public bool Value { get; set; }
}

public class ActionExecutionDelegateHelper
{
    public static ActionExecutingContext BuildContext(Dictionary<string, object?> actionArgs, string bindToBodyParamKey, BindingSource bindTo)
    {
        var httpContext = new DefaultHttpContext();
        var routeData = new RouteData();

        var actionDescriptor = new ControllerActionDescriptor
        {
            ControllerName = "Test",
            ActionName = "TestAction",
            Parameters = actionArgs.Select(x => new ParameterDescriptor
            {
                Name = x.Key,
                BindingInfo = new BindingInfo
                {
                    BindingSource = bindToBodyParamKey == x.Key ? bindTo : BindingSource.Custom
                }
            }).ToList()
        };

        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
        var filters = new List<IFilterMetadata>();
        return new ActionExecutingContext(actionContext, filters, actionArgs, null!);
    }

    public static ActionExecutionDelegate MakeNext(Flag flag)
    {
        flag.Value = false;
        return () =>
        {
            flag.Value = true;
            var context = new ActionExecutedContext(new ActionContext(
                    new DefaultHttpContext(),
                    new RouteData(),
                    new ActionDescriptor()
                ),
                new List<IFilterMetadata>(),
                null!);

            return Task.FromResult(context);
        };
    }
}
