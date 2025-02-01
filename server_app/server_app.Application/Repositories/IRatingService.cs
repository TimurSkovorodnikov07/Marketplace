namespace server_app.Application.Repositories;

public interface IRatingService
{
    Task<bool> SawCategory(Guid customerId, Guid productCategoryId);
    Task<bool> Purchased(Guid buyerId, Guid purchasedCategoryId, int numberOfPurchases);
    Task<bool> AddCommonRating(Guid categoryId);
}