namespace UseValidator;

using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class UseQueryValidator : UseValidator
{
    protected override object? GetPayload(ActionExecutingContext context)
    {
        bool IsFromQuery(ParameterDescriptor p) => p.BindingInfo != null &&
            p.BindingInfo.BindingSource != null &&
            p.BindingInfo.BindingSource.Id == BindingSource.Query.Id;

        var fromQueryParams = context.ActionDescriptor.Parameters.Where(IsFromQuery).ToList();
        if (fromQueryParams.Count != 1)
        {
            throw new InvalidOperationException("UseQueryValidator requires exactly one parameter decorated with [FromQuery]");
        }

        var ArgumentName = fromQueryParams[0].Name;
        context.ActionArguments.TryGetValue(ArgumentName, out var query);
        return query;
    }
}
