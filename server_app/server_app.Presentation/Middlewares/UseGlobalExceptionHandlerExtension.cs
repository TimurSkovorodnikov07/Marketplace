namespace server_app.Presentation.Middlewares;

public static class UseGlobalExceptionHandlerExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware>();
    }
}