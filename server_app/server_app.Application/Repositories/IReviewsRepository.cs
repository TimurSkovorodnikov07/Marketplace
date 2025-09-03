using server_app.Domain.Entities.ProductCategories.Reviews;
using server_app.Domain.Model;

namespace server_app.Application.Repositories;

public interface IReviewsRepository
{
    Task<ReviewEntity?> Get(Guid guid);
    Task<ReviewEntity?>  GetByOwnerAndCategoryIdentifiers(Guid categoryId, Guid ownerid);
    Task<ReviewsModel?> GetReviews(Guid categoryId, int from, int to, Guid? excludeReviewId);
    Task Add(ReviewEntity entity);
    Task<Result> Update(Guid guid, string newText, int newEstimation, Guid ownerGud);
    Task<Result> Remove(ReviewEntity deletedReview);
}