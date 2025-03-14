using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server_app.Application.Repositories;
using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Entities.ProductCategories.PurchasedProducts;
using server_app.Domain.Model;
using server_app.Domain.Model.Dtos;
using server_app.Infrastructure.Repositories.Users;

namespace server_app.Infrastructure.Repositories.ProductCategories;

public class ProductCategoryRepository(
    ILogger<ProductCategoryRepository> logger,
    MainDbContext dbContext,
    IMapper mapper,
    IDeliveryCompanyRepository deliveryCompanyRepository,
    ICustomerRepository customerRepository,
    ICreditCardRepository creditCardRepository,
    IRatingService ratingService,
    IImageRepository imageRepository)
    : IProductCategoryRepository
{
    public async Task<IEnumerable<ProductCategorySmallDtoForViewer>> GetRecommendation()
    {
        //Напишу систему рекомендаций чуть позже

        return dbContext.ProductsCategories
            .Include(c => c.DeliveryCompany)
            .Include(c => c.Owner)
            .Select(c => mapper.Map<ProductCategorySmallDtoForViewer>(c));
    }

    public async Task<IEnumerable<ProductCategorySmallDtoForViewer>> GetRecommendationByTag(string tag)
    {
        return dbContext.ProductsCategories
            .Include(c => c.DeliveryCompany)
            .Include(c => c.Owner)
            .Where(p => p.Tags.Tags.Any(x => x == tag))
            .Select(c => mapper.Map<ProductCategorySmallDtoForViewer>(c));
    }

    public async Task<(IEnumerable<ProductCategoryDtoForViewer> categories, int maxNumber)> GetCategoriesByViewer(
        Guid ownerId, int from, int to, string? search, int priceNoMoreThenOrEqual = 0) =>
        await GetCategories<ProductCategoryDtoForViewer>(ownerId, from, to, search,
            priceNoMoreThenOrEqual);

    public async Task<(IEnumerable<ProductCategoryDtoForOwner>categories, int maxNumber)> GetCategoriesByOwner(
        Guid ownerId, int from, int to, string? search, int priceNoMoreThenOrEqual = 0) =>
        await GetCategories<ProductCategoryDtoForOwner>(ownerId, from, to, search,
            priceNoMoreThenOrEqual);

    public async Task<ProductCategoryEntity?> Get(Guid guid)
    {
        return await dbContext.ProductsCategories
            .Include(p => p.DeliveryCompany)
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(p => p.Id == guid);
    }

    public async Task<ProductCategoryEntity?> GetWithoutOtherData(Guid guid)
    {
        return await dbContext.ProductsCategories
            .FirstOrDefaultAsync(p => p.Id == guid);
    }

    public async Task<(IEnumerable<PurchasedProductDto> products, int maxNumber)> GetPurchasedProducts(
        Guid buyerId, int from, int to)
    {
        var query = dbContext.PurchasedProducts
            .Include(c => c.Category)
            .Where(c => c.BuyerId == buyerId);

        var totalResult = await query
            .Select(p => mapper.Map<PurchasedProductDto>(p))
            .ToListAsync();
        var result = totalResult
            .Skip(from)
            .Take(to - from);

        return (result, totalResult.Count());
    }

    public async Task<bool> EstimationUpdate(Guid categoryId)
    {
        var categoryEntity = await Get(categoryId);

        if (categoryEntity == null)
            return false;

        var estimations = await dbContext.Reviews
            .Where(x => x.CategoryId == categoryId)
            .Select(x => x.Estimation)
            .ToListAsync();

        var count = estimations.Count;
        var totalEstimation = estimations.Sum();

        if (count > 0)
        {
            categoryEntity.TotalEstimation = (int)totalEstimation / count;
            categoryEntity.EstimationCount = count;
        }
        else
        {
            categoryEntity.TotalEstimation = 0;
            categoryEntity.EstimationCount = 0;
        }

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsBought(Guid categoryId, Guid buyerId)
    {
        return await dbContext.PurchasedProducts
            .AnyAsync(x => x.CategoryId == categoryId
                           && x.BuyerId == buyerId);
    }

    public async Task<bool> NameIsFree(Guid ownerGuid, string name)
    {
        var nameToLower = name.ToLower();

        return await dbContext.ProductsCategories
            .AnyAsync(c => c.OwnerId == ownerGuid
                           && c.Name.ToLower() == nameToLower) == false;
    }

    public async Task<Result> Add(ProductCategoryCreateDto createdCategory)
    {
        await dbContext.Database.BeginTransactionAsync();

        try
        {
            var newCategory = ProductCategoryEntity.Create(createdCategory);

            if (newCategory == null)
                return await BadRequestResultReturnAndTransactionCanceled("New category not valid");

            var (saveIsSuccesses, mainImageId) =
                await imageRepository.Save(createdCategory.Images, newCategory.Id);

            if (saveIsSuccesses == false || mainImageId == null)
                return await BadRequestResultReturnAndTransactionCanceled(
                    "Images could not be saved, they may not be valid");

            newCategory.MainImageId = (Guid)mainImageId;
            await dbContext.ProductsCategories.AddAsync(newCategory);
            await dbContext.SaveChangesAsync();
            
            await ratingService.AddCommonRating(newCategory.Id);
            await dbContext.SaveChangesAsync();
            await dbContext.Database.CommitTransactionAsync();

            return Result.Ok(newCategory.Id);
        }
        catch (Exception e)
        {
            logger.LogError("Transaction did not work: " + e.Message);
            await dbContext.Database.RollbackTransactionAsync();

            return Result.InternalServerError();
        }
    }

    public async Task<bool> Update(ProductCategoryUpdateDto updatedCategory)
    {
        var category = await Get(updatedCategory.Id);
        var newCompany = await deliveryCompanyRepository.Get(updatedCategory.NewDeliveryCompanyId);

        if (category == null || newCompany == null)
            return false;

        category.Name = updatedCategory.NewName;
        category.Description = updatedCategory.NewDescription;
        category.Tags = updatedCategory.NewTags;
        category.Price = updatedCategory.NewPrice;
        category.Quantity = updatedCategory.NewQuantity;
        category.DeliveryCompany = newCompany;
        //Стоит такой пиздец лучше маппить, но это потом

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Result> Buy(List<CategoryBuyDto> purchasedCategoriesDtos, Guid buyerId)
    {
        await dbContext.Database.BeginTransactionAsync();

        try
        {
            //1) Находим покупателя
            var foundBuyer = await customerRepository.Get(buyerId);
            if (foundBuyer == null)
            {
                logger.LogCritical("Разраб гондон, передал хуй пойми какой Id не существующего юзера");
                return await ResultReturnAndTransactionCanceled(Result.NotFound("Customer not found"));
            }

            if (foundBuyer.CreditCard == null)
                return await ResultReturnAndTransactionCanceled(
                    Result.BadRequest("The buyer does not have a credit card"));

            decimal totalSumForAllProducts = 0;

            foreach (var dto in purchasedCategoriesDtos)
            {
                //2) Находим категорию товаров
                var foundCategory = await Get(dto.PurchasedCategoryId);
                if (foundCategory == null)
                    return await ResultReturnAndTransactionCanceled(Result.NotFound("Category not found"));

                if (foundCategory.Quantity < dto.NumberOfPurchases)
                    return await BadRequestResultReturnAndTransactionCanceled(
                        $"You cannot buy {dto.NumberOfPurchases} piece this product(there are only {foundCategory.Quantity} pieces)");

                decimal totalSum = dto.NumberOfPurchases * foundCategory.Price;
                totalSumForAllProducts += totalSum;
                // 3) Создаем "Купленный" продукт

                var randomDays = new Random().Next(1, 15);
                var mustDeliveredBefore = DateTime.UtcNow.AddDays(randomDays);
                var newPurchasedProduct = PurchasedProductEntity.Create(foundCategory, foundBuyer, mustDeliveredBefore,
                    dto.NumberOfPurchases, totalSum, foundCategory.MainImageId);

                // 4)  Товар  добавлен, колл. товаров стало меньше
                await dbContext.PurchasedProducts.AddAsync(newPurchasedProduct);
                foundCategory.Quantity -= dto.NumberOfPurchases;
                await ratingService.Purchased(buyerId: foundBuyer.Id, purchasedCategoryId: foundCategory.Id,
                    numberOfPurchases: dto.NumberOfPurchases);
            }

            //5) Берем деньги из карты
            var purchaseResult =
                await creditCardRepository.IsEnoughMoneyAndWriteOff(foundBuyer.CreditCard.Id, totalSumForAllProducts);
            if (purchaseResult == false)
                return await ResultReturnAndTransactionCanceled(Result.PaymentRequired());

            await dbContext.SaveChangesAsync();
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

    public async Task<bool> Remove(Guid guid)
    {
        var deletedCategory = await Get(guid);

        if (deletedCategory == null)
            return false;

        await imageRepository.DeleteAllByProductCategoryId(guid);
        dbContext.ProductsCategories.Remove(deletedCategory);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Exists(Guid guid) => await dbContext.ProductsCategories
        .AnyAsync(c => c.Id == guid);


    private async Task<(IEnumerable<ReturnType> categories, int maxNumber)> GetCategories<ReturnType>(Guid ownerId,
        int from, int to, string? search = null, int priceNoMoreThenOrEqual = 0) where ReturnType : ProductCategoryDto
    {
        var query = dbContext.ProductsCategories
            .Include(c => c.DeliveryCompany)
            .Include(c => c.Owner)
            .Where(c => c.OwnerId == ownerId
                        && (priceNoMoreThenOrEqual == 0 || c.Price <= priceNoMoreThenOrEqual)
                        && c.Quantity > 0);

        if (!string.IsNullOrEmpty(search))
        {
            var searchToLower = search.ToLower();
            query = query.Where((p) => p.Name.ToLower().Contains(searchToLower));
        }

        var totalResult = await query
            .Select(p => mapper.Map<ReturnType>(p))
            .ToListAsync();
        var result = totalResult
            .Skip(from)
            .Take(to - from);

        return (result, totalResult.Count());
    }


    private async Task<Result> BadRequestResultReturnAndTransactionCanceled(object? value = null) =>
        await ResultReturnAndTransactionCanceled(Result.BadRequest(value));

    private async Task<Result> ResultReturnAndTransactionCanceled(Result result)
    {
        await dbContext.Database.RollbackTransactionAsync();
        return result;
    }
}