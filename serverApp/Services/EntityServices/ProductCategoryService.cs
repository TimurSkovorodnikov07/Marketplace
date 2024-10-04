using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class ProductCategoryService(
    ILogger<ProductCategoryService> logger,
    MainDbContext dbContext,
    IMapper mapper,
    DeliveryCompanyService deliveryCompanyService,
    SellerService sellerService,
    ImagesService imagesService,
    CustomerService customerService,
    CreditCardService creditCardService)
    : IEntityService<ProductCategoryEntity, ProductCategoryCreateDto, ProductCategoryUpdateDto,
        Result, bool, bool>
{
    public async Task<IEnumerable<ProductCategoryDtoForViewer>> GetCategoriesByViewer(Guid ownerId,
        GetCategoriesQuery query) =>
        await GetCategories<ProductCategoryDtoForViewer>(ownerId, query.From, query.To, query.Search,
            query.PriceNoMoreThenOrEqual);

    public async Task<IEnumerable<ProductCategoryDtoForOwner>> GetCategoriesByOwner(Guid ownerId,
        GetCategoriesQuery query) =>
        await GetCategories<ProductCategoryDtoForOwner>(ownerId, query.From, query.To, query.Search,
            query.PriceNoMoreThenOrEqual);

    public async Task<ProductCategoryEntity?> Get(Guid guid)
    {
        return await dbContext.ProductsCategory
            .Include(p => p.DeliveryCompany)
            .Include(p => p.Owner)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == guid);
    }

    public async Task<Result> Add(ProductCategoryCreateDto createdCategory)
    {
        //К сожалению мне придеться юзать класс Result как возвращаемое значение тк тут транзакция
        await dbContext.Database.BeginTransactionAsync();
        try
        {
            var newCategory = ProductCategoryEntity.Create(createdCategory);

            if (newCategory == null)
                return await BadRequestResultReturnAndTransactionCanceled("New category not valid");

            await dbContext.ProductsCategory.AddAsync(newCategory);
            await dbContext.SaveChangesAsync();

            var imagesSaveResult = await imagesService.Save(createdCategory.Images, newCategory.Id, false);

            if (!imagesSaveResult)
                return await BadRequestResultReturnAndTransactionCanceled(
                    "Images could not be saved, they may not be valid");

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

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Result> Buy(ProductBuyQuery query, Guid buyerId)
    {
        await dbContext.Database.BeginTransactionAsync();

        try
        {
            //1) Находим категорию товаров
            var foundCategory = await Get(query.ProductCategoryId);
            if (foundCategory == null)
                return await ResultReturnAndTransactionCanceled(Result.NotFound("Category not found"));

            if (foundCategory.Quantity <= 0)
                return await BadRequestResultReturnAndTransactionCanceled("There are no products in this category");

            //2) Находим покупателя
            var foundBuyer = await customerService.Get(buyerId);
            if (foundBuyer == null)
            {
                logger.LogCritical("Разраб гондон, передал хуй пойми какой Id не существующего юзера");
                return await ResultReturnAndTransactionCanceled(Result.NotFound("Customer not found"));
            }

            //3) Берем деньги из карты
            var purchaseIsSuccessed =
                await creditCardService.IsEnoughManyAndWriteOff(foundBuyer.CreditCardId, foundCategory.Price);
            if (!purchaseIsSuccessed)
                return await ResultReturnAndTransactionCanceled(Result.PaymentRequired());

            await dbContext.SaveChangesAsync();


            // 4) Создаем "Купленный" продукт, забыл сказать, до покупки так таковых продуктов нету,
            // почему? А потому что мне это нахуй не надо, ведь у меня нету РЕАЛЬНОГО взаимодествия с deliveryCompany которым каждый продукт важен
            // У меня просто есть категория, и число продуктов в ней, до покупки продукты не создаю
            //Как нибудь поменяю когда проект изменю сделая его больше похожим на РЕАЛЬНЫЙ маркетплейс
            var newPurchasedProduct = PurchasedProductEntity.Create(foundCategory, foundBuyer,
                DateTime.UtcNow.AddDays(new Random().Next(1, 15)));
            //Имитация, блять, по другому будет пиздец долго, скорее всего мой маркетплейс юзать не будут
            //А в идиале нужно запрашивать инфу у апи deliveryCompany

            if (newPurchasedProduct == null)
                return await BadRequestResultReturnAndTransactionCanceled("Product not valid");

            // 5)  Товар куплен, колл товаров стало меньше
            foundCategory.Quantity--;
            await dbContext.PurchasedProducts.AddAsync(newPurchasedProduct);
            await dbContext.SaveChangesAsync();

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

        dbContext.ProductsCategory.Remove(deletedCategory);
        await dbContext.SaveChangesAsync();

        return true;
    }


    private async Task<IEnumerable<ReturnType>> GetCategories<ReturnType>(Guid ownerId,
        int from, int to, string? search = null, int priceNoMoreThenOrEqual = 0) where ReturnType : ProductCategoryDto
    {
        //В прев проекте я не думаю нахерачил кучу ToList() что являеться очень тупой и опастной штукой
        //Если кратко есть методы вроде ToList(), ToArray(), Single(), First(), Count(), Sum(), Max(), Min(), Average() и тому подобные
        //Прикод этих меодов в том что они ВОЗВРАЩАЮТ ДАННЫЕ, именно возвращают а не создают запрос как тот же Where или OrderBy
        var list = dbContext.ProductsCategory
            .Include(p => p.DeliveryCompany)
            .Include(p => p.Owner)
            .Include(p => p.Images)
            .Where(p => p.OwnerId == ownerId)
            .Skip(from)
            .Take(to - from)
            .Select(p => mapper.Map<ReturnType>(p));

        if (priceNoMoreThenOrEqual > 0)
        {
            list.Where(c => c.Price <= priceNoMoreThenOrEqual);
        }

        if (!string.IsNullOrEmpty(search))
        {
            string searchToLower = search.ToLower();
            list.Where((p) => p.Name.ToLower().Contains(searchToLower));
        }


        return await list.ToListAsync();
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