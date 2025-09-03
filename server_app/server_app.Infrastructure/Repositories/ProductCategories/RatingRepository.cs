using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server_app.Application.Options;
using server_app.Application.Repositories;
using server_app.Domain.Entities.ProductCategories.Ratings;

namespace server_app.Infrastructure.Repositories.ProductCategories;

public class RatingRepository(
    MainDbContext context,
    IOptions<RatingForceOptions> rattingForceOptions,
    ILogger<RatingRepository> logger) : IRatingService
{
    private readonly RatingForceOptions _ratingForce = rattingForceOptions.Value;

    public async Task<bool> SawCategory(Guid customerId, Guid productCategoryId)
    {
        var addRating = _ratingForce.InterestedForce;
        var foundCommonRating = await GetCommonRating(productCategoryId);

        if (foundCommonRating == null)
            return false;

        var foundRating = await Get(productCategoryId: productCategoryId, customerId: customerId);
        if (foundRating != null) return true;
        
        var newRattingFromCustomer =
            RatingFromCustomerEntity.Create(customerId: customerId, commonRating: foundCommonRating);

        if (newRattingFromCustomer == null)
            return false;

        newRattingFromCustomer.Rating += addRating;
        foundCommonRating.TotalRating += addRating;
        await context.RattingFromCustomers.AddAsync(newRattingFromCustomer);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Purchased(Guid buyerId, Guid purchasedCategoryId, int numberOfPurchases)
    {
        var addRating = _ratingForce.PurchasedForce * numberOfPurchases;
        var foundCommonRating = await GetCommonRating(productCategoryId: purchasedCategoryId);

        if (foundCommonRating == null)
            return false;

        var foundRating = await Get(productCategoryId: purchasedCategoryId, customerId: buyerId);
        if (foundRating == null)
        {
            var newRattingFromCustomer =
                RatingFromCustomerEntity.Create(customerId: buyerId, commonRating: foundCommonRating);

            if (newRattingFromCustomer == null)
                return false;

            newRattingFromCustomer.Rating += addRating;
            await context.RattingFromCustomers.AddAsync(newRattingFromCustomer);
        }
        else
        {
            foundRating.Rating += addRating;
        }
        foundCommonRating.TotalRating += addRating;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddCommonRating(Guid categoryId)
    {
        var newCommonRatting = RatingEntity.Create(categoryId);

        if (newCommonRatting == null)
            return false;

        await context.Rattings.AddAsync(newCommonRatting);
        await context.SaveChangesAsync();
        return true;
    }
    
    
    // The Methods for this service:  
    private async Task Add(RatingFromCustomerEntity newEntity)
    {
        await context.RattingFromCustomers.AddAsync(newEntity);
        await context.SaveChangesAsync();
    }
    private async Task<RatingFromCustomerEntity?> Get(Guid productCategoryId, Guid customerId)
    {
        return await context.RattingFromCustomers
            .Include(x => x.CommonRating)
            .FirstOrDefaultAsync(x => x.CommonRating.ProductCategoryId == productCategoryId
                                      && x.CustomerId == customerId);
    }
    private async Task<RatingEntity?> GetCommonRating(Guid productCategoryId)
    {
        return await context.Rattings
            .FirstOrDefaultAsync(x => x.ProductCategoryId == productCategoryId);
    }
}