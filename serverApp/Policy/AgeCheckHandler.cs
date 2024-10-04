using Microsoft.AspNetCore.Authorization;

public class AgeCheckHandler(ILogger<AgeCheckHandler> logger, ProductCategoryService productCategoryService)
    : AuthorizationHandler<AgeRequirement>
{
    private readonly ILogger<AgeCheckHandler> _logger = logger;
    private readonly ProductCategoryService _productCategoryService = productCategoryService;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        throw new Exception();
    }
}