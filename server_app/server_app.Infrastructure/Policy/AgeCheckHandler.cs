using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using server_app.Application.Services.EntitiesServices;
using server_app.Application.Services.EntitiesServices.Interfaces;

namespace server_app.Infrastructure.Policy;

public class AgeCheckHandler(ILogger<AgeCheckHandler> logger, IProductCategoryService productCategoryService)
    : AuthorizationHandler<AgeRequirement>
{
    private readonly ILogger<AgeCheckHandler> _logger = logger;
    private readonly IProductCategoryService _productCategoryService = productCategoryService;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        throw new Exception();
    }
}