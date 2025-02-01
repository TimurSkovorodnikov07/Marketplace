using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Model;
using server_app.Domain.Model.Dtos;

namespace server_app.Application.Repositories;

public interface IProductCategoryRepository : IEntityRepository<ProductCategoryEntity, ProductCategoryCreateDto,
    ProductCategoryUpdateDto,
    Result, bool, bool>
{
    Task<IEnumerable<ProductCategorySmallDtoForViewer>> GetRecommendationByTag(string tag);
    Task<IEnumerable<ProductCategorySmallDtoForViewer>> GetRecommendation();

    Task<(IEnumerable<ProductCategoryDtoForViewer> categories, int maxNumber)> GetCategoriesByViewer(
        Guid ownerId, int from, int to, string? search, int priceNoMoreThenOrEqual = 0);
    Task<(IEnumerable<ProductCategoryDtoForOwner>categories, int maxNumber)> GetCategoriesByOwner(
        Guid ownerId, int from, int to, string? search, int priceNoMoreThenOrEqual = 0);

    Task<ProductCategoryEntity?> GetWithoutOtherData(Guid guid);

    Task<(IEnumerable<PurchasedProductDto> products, int maxNumber)> GetPurchasedProducts(
        Guid buyerId, int from, int to);

    Task<bool> NameIsFree(Guid ownerGuid, string name);
    Task<Result> Buy(List<CategoryBuyDto> purchasedCategoriesDtos, Guid buyerId);
    Task<bool> IsBought(Guid categoryId, Guid buyerId);
    Task<bool> EstimationUpdate(Guid categoryId);
    Task<bool> Exists(Guid guid);
}