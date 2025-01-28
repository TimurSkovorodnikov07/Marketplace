using server_app.Domain.Entities.ProductCategories.Ratings;

namespace server_app.Application.Services.EntitiesServices.Interfaces;

public interface IRatingService
{
    Task<bool> SawCategory(Guid customerId, Guid productCategoryId);
    Task<bool> Purchased(Guid buyerId, Guid purchasedCategoryId, int numberOfPurchases);
    Task<bool> AddCommonRating(Guid categoryId);
}