using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace server_app.Presentation.Filters;

public class ValidationFilter(ILogger<ValidationFilter> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;
        
        logger.LogDebug("Model is not valid");
        context.Result = new BadRequestResult();
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ValidationFilterAttribute() : ServiceFilterAttribute(typeof(ValidationFilter));