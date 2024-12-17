using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ReviewService(
    ProductCategoryService productCategoryService,
    ILogger<ReviewService> logger,
    MainDbContext dbContext,
    IMapper mapper)
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

    public async Task<ReviewsModel?> GetReviews(GetReviewsQuery dto, Guid? excludeReviewId)
    {
        var categoryIsExist = await productCategoryService.Exists(dto.CategoryId);
        if (categoryIsExist == false) return null;

        var where = (ReviewEntity r) => r.CategoryId == dto.CategoryId;

        if (excludeReviewId != null)
            where = r => r.CategoryId == dto.CategoryId
                         && r.ReviewOwnerId != (Guid)excludeReviewId;

        var totalResult = dbContext.Reviews
            .Include(r => r.Category)
            .Include(r => r.ReviewOwner)
            .Where(where)
            .Select(p => mapper.Map<ReviewDto>(p));

        var result = totalResult
            .Skip(dto.From)
            .Take(dto.To - dto.From);

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
            await productCategoryService.EstimationUpdate(entity.CategoryId);
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
            await productCategoryService.EstimationUpdate(foundReview.CategoryId);
            await dbContext.Database.CommitTransactionAsync();

            return Result.Ok();
        }
        catch (Exception e)
        {
            logger.LogError("Transaction did not work: " + e.Message);
            await dbContext.Database.RollbackTransactionAsync();
            
            return new Result(false, new StatusCodeResult(StatusCodes.Status500InternalServerError));
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
            
            var isSuccessUpdate = await productCategoryService.EstimationUpdate(categoryId);

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
        return new Result(false, new StatusCodeResult(StatusCodes.Status500InternalServerError));
    }


    private async Task<ReviewEntity?> GetWithoutLoadOtherData(Guid guid)
    {
        return await dbContext.Reviews
            .FirstOrDefaultAsync(x => x.Id == guid);
    }
}