namespace server_app.Presentation.Middlewares;

public class ExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlerMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
            //След компоненты вызываеться, если возникает ошибка мидллвеер работает в обратную сторону
            //Доходя до этого try-catch exception обрабатываеться
        }
        catch (Exception ex)
        {
            await Handle(context, ex, (int)StatusCodes.Status500InternalServerError,
                "Internal Server Error");
        }
    }
    private async Task Handle(HttpContext context, Exception exception,
        int statusCode, string message)
    {
        var response = context.Response;
        response.StatusCode = statusCode;
        response.ContentType = "application/json";

        logger.LogCritical("An exception occurred on the server, exception mes: {m}", exception.Message);

        var exceptionDto = new
        {
            statusCode = statusCode,
            message = message,
            exceptionMessage = exception.Message
        };
        await response.WriteAsJsonAsync(exceptionDto);
    }
}