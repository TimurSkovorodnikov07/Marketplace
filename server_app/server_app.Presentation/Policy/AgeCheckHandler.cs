using Microsoft.AspNetCore.Authorization;
using server_app.Application.Repositories;

namespace server_app.Presentation.Policy;

public class AgeCheckHandler(ILogger<AgeCheckHandler> logger, IProductCategoryRepository productCategoryRepository)
    : AuthorizationHandler<AgeRequirement>
{
    private readonly ILogger<AgeCheckHandler> _logger = logger;
    private readonly IProductCategoryRepository _productCategoryRepository = productCategoryRepository;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        throw new Exception();
    }
}