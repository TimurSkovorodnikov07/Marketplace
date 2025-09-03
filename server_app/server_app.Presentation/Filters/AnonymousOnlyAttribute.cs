using Microsoft.AspNetCore.Mvc.Filters;

namespace server_app.Presentation.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AnonymousOnlyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context?.HttpContext?.User?.Identity?.IsAuthenticated == true)
            context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
    }
}