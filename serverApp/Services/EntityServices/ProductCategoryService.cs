using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductCategoryService(
    ILogger<ProductCategoryService> logger,
    MainDbContext dbContext,
    IMapper mapper,
    DeliveryCompanyService deliveryCompanyService,
    ImagesService imagesService,
    CustomerService customerService,
    CreditCardService creditCardService,
    RatingService ratingService)
    : IEntityService<ProductCategoryEntity, ProductCategoryCreateDto, ProductCategoryUpdateDto,
        Result, bool, bool>
{
    public async Task<IEnumerable<ProductCategoryDtoForViewer>> GetRecommendation()
    {
        //Напишу систему рекомендаций чуть позже
        
        return  dbContext.ProductsCategories
            .Include(c => c.DeliveryCompany)
            .Include(c => c.Owner)
            .Include(c => c.Images)
            .Select(c => mapper.Map<ProductCategoryDtoForViewer>(c));
    }

    public async Task<IEnumerable<ProductCategoryDtoForViewer>> GetRecommendationByTag(string tag)
    {
        return  dbContext.ProductsCategories
            .Include(c => c.DeliveryCompany)
            .Include(c => c.Owner)
            .Include(c => c.Images)
            .Where(p => p.Tags.Tags.Any(x => x == tag))
            .Select(c => mapper.Map<ProductCategoryDtoForViewer>(c));
    }

    public async Task<(IEnumerable<ProductCategoryDtoForViewer> categories, int maxNumber)> GetCategoriesByViewer(
        Guid ownerId,
        GetCategoriesQuery query) =>
        await GetCategories<ProductCategoryDtoForViewer>(ownerId, query.From, query.To, query.Search,
            query.PriceNoMoreThenOrEqual);

    public async Task<(IEnumerable<ProductCategoryDtoForOwner>categories, int maxNumber)> GetCategoriesByOwner(
        Guid ownerId,
        GetCategoriesQuery query) =>
        await GetCategories<ProductCategoryDtoForOwner>(ownerId, query.From, query.To, query.Search,
            query.PriceNoMoreThenOrEqual);

    public async Task<ProductCategoryEntity?> Get(Guid guid)
    {
        return await dbContext.ProductsCategories
            .Include(p => p.DeliveryCompany)
            .Include(p => p.Owner)
            .Include(p => p.Images)
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
            .ThenInclude(c => c.Images)
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

            await dbContext.ProductsCategories.AddAsync(newCategory);
            await dbContext.SaveChangesAsync();

            var saveIsSuccesses =
                await imagesService.Save(newCategory.Id, createdCategory.Images);

            if (!saveIsSuccesses)
                return await BadRequestResultReturnAndTransactionCanceled(
                    "Images could not be saved, they may not be valid");

            await ratingService.AddCommonRating(newCategory.Id);
            await dbContext.SaveChangesAsync();
            await dbContext.Database.CommitTransactionAsync();

            return new Result(true, new OkObjectResult(newCategory.Id));
        }
        catch (Exception e)
        {
            logger.LogError("Transaction did not work: " + e.Message);
            await dbContext.Database.RollbackTransactionAsync();

            return new Result(false, new StatusCodeResult(StatusCodes.Status500InternalServerError));
        }
    }

    public async Task<bool> Update(ProductCategoryUpdateDto updatedCategory)
    {
        var category = await Get(updatedCategory.Id);
        var newCompany = await deliveryCompanyService.Get(updatedCategory.NewDeliveryCompanyId);

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
            var foundBuyer = await customerService.Get(buyerId);
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
                // 3) Создаем "Купленный" продукт, забыл сказать, до покупки так таковых продуктов нету,
                // почему? А потому что мне это нахуй не надо, ведь у меня нету РЕАЛЬНОГО взаимодествия с deliveryCompany которым каждый продукт важен
                // У меня просто есть категория, и число продуктов в ней, до покупки продукты не создаю
                //Как нибудь поменяю когда проект изменю сделая его больше похожим на РЕАЛЬНЫЙ маркетплейс

                var randomDays = new Random().Next(1, 15);
                var mustDeliveredBefore = DateTime.UtcNow.AddDays(randomDays);
                var newPurchasedProduct = PurchasedProductEntity.Create(foundCategory, foundBuyer, mustDeliveredBefore,
                    dto.NumberOfPurchases, totalSum);
                //Имитация, блять, по другому будет пиздец долго, скорее всего мой маркетплейс юзать не будут
                //А в идиале нужно запрашивать инфу у апи deliveryCompany

                // 4)  Товар  добавлен, колл товаров стало меньше
                await dbContext.PurchasedProducts.AddAsync(newPurchasedProduct);
                foundCategory.Quantity -= dto.NumberOfPurchases;
                await ratingService.Purchased(buyerId: foundBuyer.Id, purchasedCategoryId: foundCategory.Id,
                    numberOfPurchases: dto.NumberOfPurchases);
            }

            //5) Берем деньги из карты
            var purchaseResult =
                await creditCardService.IsEnoughMoneyAndWriteOff(foundBuyer.CreditCard.Id, totalSumForAllProducts);
            if (purchaseResult == false)
                return await ResultReturnAndTransactionCanceled(Result.PaymentRequired());

            await dbContext.SaveChangesAsync();
            await dbContext.Database.CommitTransactionAsync();

            return new Result(true);
        }
        catch (Exception e)
        {
            logger.LogError("Transaction did not work: " + e.Message);
            await dbContext.Database.RollbackTransactionAsync();

            return new Result(false, new StatusCodeResult(StatusCodes.Status500InternalServerError));
        }
    }

    public async Task<bool> Remove(Guid guid)
    {
        var deletedCategory = await Get(guid);

        if (deletedCategory == null)
            return false;

        dbContext.ProductsCategories.Remove(deletedCategory);
        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Exists(Guid guid) => await dbContext.ProductsCategories
        .AnyAsync(c => c.Id == guid);


    private async Task<(IEnumerable<ReturnType> categories, int maxNumber)> GetCategories<ReturnType>(Guid ownerId,
        int from, int to, string? search = null, int priceNoMoreThenOrEqual = 0) where ReturnType : ProductCategoryDto
    {
        //В прев проекте я не думаю нахерачил кучу ToList() что являеться очень тупой и опастной штукой
        //Если кратко есть методы вроде ToList(), ToArray(), Single(), First(), Count(), Sum(), Max(), Min(), Average() и тому подобные
        //Прикод этих меодов в том что они ВОЗВРАЩАЮТ ДАННЫЕ, именно возвращают а не создают запрос как тот же Where или OrderBy
        var query = dbContext.ProductsCategories
            .Include(c => c.DeliveryCompany)
            .Include(c => c.Owner)
            .Include(c => c.Images)
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


    //Методы чтобы дохуя кода не писать в методах с транзакциями
    private async Task<Result> BadRequestResultReturnAndTransactionCanceled(object? value = null) =>
        await ResultReturnAndTransactionCanceled(Result.BadRequest(value));

    private async Task<Result> ResultReturnAndTransactionCanceled(Result result)
    {
        await dbContext.Database.RollbackTransactionAsync();
        return result;
    }
}