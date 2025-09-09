namespace UseValidator;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public abstract class UseValidator : ActionFilterAttribute
{
    public required Type Validator { get; set; }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var payload = GetPayload(context);
        if (payload != null)
        {
            var result = Validate(payload);
            if (result == null || !result.IsValid)
            {
                var errors = result == null ? [] : result.Errors;
                context.Result = new BadRequestObjectResult(errors);
                return;
            }
        }

        await next();
    }

    private ValidationResult? Validate(object payload)
    {
        var validatorInstance = Activator.CreateInstance(Validator);
        var method = validatorInstance!.GetType()
            .GetMethods()
            .FirstOrDefault(m => m.Name == nameof(IValidator<object>.ValidatePayload));

        var result = method!.Invoke(validatorInstance, [payload]);
        return result as ValidationResult;
    }

    protected abstract object? GetPayload(ActionExecutingContext context);
}
