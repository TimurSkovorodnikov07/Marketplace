using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server_app.Application.Repositories;
using server_app.Domain.Entities.ProductCategories.Reviews;
using server_app.Domain.Model;
using server_app.Domain.Model.Dtos;

namespace server_app.Infrastructure.Repositories.ProductCategories;

public class ReviewsRepository(
    IProductCategoryRepository productCategoryRepository,
    ILogger<ReviewsRepository> logger,
    MainDbContext dbContext,
    IMapper mapper) : IReviewsRepository
{
    public async Task<ReviewEntity?> Get(Guid guid)
    {
        return await dbContext.Reviews
            .Include(r => r.Category)
            .Include(x => x.ReviewOwner)
            .FirstOrDefaultAsync(x => x.Id == guid);
    }

    public async Task<ReviewEntity?> GetByOwnerAndCategoryIdentifiers(Guid categoryId, Guid ownerid)
    {
        return await dbContext.Reviews
            .Include(r => r.Category)
            .Include(x => x.ReviewOwner)
            .FirstOrDefaultAsync(x => x.ReviewOwnerId == ownerid
                                      && x.CategoryId == categoryId);
    }

    public async Task<ReviewsModel?> GetReviews(Guid categoryId, int from, int to, Guid? excludeReviewId)
    {
        var categoryIsExist = await productCategoryRepository.Exists(categoryId);
        if (categoryIsExist == false) return null;

        var where = (ReviewEntity r) => r.CategoryId == categoryId;

        if (excludeReviewId != null)
            where = r => r.CategoryId == categoryId
                         && r.ReviewOwnerId != (Guid)excludeReviewId;

        var totalResult = dbContext.Reviews
            .Include(r => r.Category)
            .Include(r => r.ReviewOwner)
            .Where(where)
            .Select(p => mapper.Map<ReviewDto>(p));

        var result = totalResult
            .Skip(from)
            .Take(to - from);

        return new ReviewsModel
        {
            ReviewDtos = result,
            MaxCount = totalResult.Count()
        };
    }

    public async Task Add(ReviewEntity entity)
    {
        await dbContext.Database.BeginTransactionAsync();
        try
        {
            await dbContext.Reviews.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            await productCategoryRepository.EstimationUpdate(entity.CategoryId);
            await dbContext.Database.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            logger.LogError("Transaction did not work: " + e.Message);
            await dbContext.Database.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<Result> Update(Guid guid, string newText, int newEstimation, Guid ownerGud)
    {
        var foundReview = await GetWithoutLoadOtherData(guid);

        if (foundReview == null)
            return Result.NotFound("Review not found");

        if (foundReview.ReviewOwnerId != ownerGud)
            return Result.Forbid();

        await dbContext.Database.BeginTransactionAsync();
        try
        {
            foundReview.Text = newText;
            foundReview.Estimation = newEstimation;

            await dbContext.SaveChangesAsync();
            await productCategoryRepository.EstimationUpdate(foundReview.CategoryId);
            await dbContext.Database.CommitTransactionAsync();

            return Result.Ok();
        }
        catch (Exception e)
        {
            logger.LogError("Transaction did not work: " + e.Message);
            await dbContext.Database.RollbackTransactionAsync();

            return Result.InternalServerError();
        }
    }
    public async Task<Result> Remove(ReviewEntity deletedReview)
    {
        await dbContext.Database.BeginTransactionAsync();
        
        try
        {
            var categoryId = deletedReview.CategoryId;
            dbContext.Reviews.Remove(deletedReview);
            await dbContext.SaveChangesAsync();
            
            var isSuccessUpdate = await productCategoryRepository.EstimationUpdate(categoryId);

            if (isSuccessUpdate)
            {
                await dbContext.Database.CommitTransactionAsync();
                return Result.Ok();
            }
        }
        catch (Exception e)
        {
            logger.LogError("Transaction did not work: " + e.Message);
            await dbContext.Database.RollbackTransactionAsync();
        }
        return Result.InternalServerError();
    }


    private async Task<ReviewEntity?> GetWithoutLoadOtherData(Guid guid)
    {
        return await dbContext.Reviews
            .FirstOrDefaultAsync(x => x.Id == guid);
    }
}