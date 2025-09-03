using Microsoft.AspNetCore.Mvc;
using server_app.Application.Exceptions;
using server_app.Domain.Model;
using server_app.Presentation.Controllers;

namespace server_app.Presentation.Extensions;

public static class ResultExtensions
{
    public static IActionResult ResultToIActionResult(this Result result, ILogger<ProductsController> logger = null)
    {
        logger?.LogInformation($"Returning {result.HttpCode}: {result.Value}");
        
        return result.HttpCode switch
        {
            200 => Ok(result.Value),
            400 => BadRequest(result.Value),
            402 => new StatusCodeResult(StatusCodes.Status402PaymentRequired),
            403 => new ForbidResult(),
            404 => NotFound(result.Value),
            500 => new StatusCodeResult(StatusCodes.Status500InternalServerError),
            _ => throw new UnexpectedHttpCode($"Unexpected http code: {result.HttpCode}"),
        };
    }

    private static IActionResult Ok(object? value) =>
        value == null
            ? new OkResult()
            : new OkObjectResult(value);

    private static IActionResult NotFound(object? value) =>
        value == null
            ? new NotFoundResult()
            : new NotFoundObjectResult(value);

    private static IActionResult BadRequest(object? value) =>
        value == null
            ? new BadRequestResult()
            : new BadRequestObjectResult(value);
}