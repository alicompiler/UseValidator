namespace UseValidator;

using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class UseBodyValidator : UseValidator
{
    protected override object GetPayload(ActionExecutingContext context)
    {
        bool IsFromBody(ParameterDescriptor p)
        {
            return p.BindingInfo != null &&
                p.BindingInfo.BindingSource != null &&
                p.BindingInfo.BindingSource.Id == BindingSource.Body.Id;
        }

        var fromBodyParams = context.ActionDescriptor.Parameters.Where(IsFromBody).ToList();
        if (fromBodyParams.Count != 1)
        {
            throw new InvalidOperationException("UseBodyValidator requires exactly one parameter decorated with [FromBody]");
        }

        var ArgumentName = fromBodyParams[0].Name;
        context.ActionArguments.TryGetValue(ArgumentName, out var body);

        if (body == null)
        {
            throw new InvalidOperationException($"The ActionArgument '{ArgumentName}' is null. Ensure that the action is called with the appropriate body.");
        }

        return body;
    }
}
