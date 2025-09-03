using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server_app.Application.Extensions;
using server_app.Application.Repositories;
using server_app.Application.Services;
using server_app.Domain.Model.Dtos;
using server_app.Presentation.Extensions;
using server_app.Presentation.Filters;
using server_app.Presentation.Filters.ValidatorAttributes;
using server_app.Presentation.ModelQueries;

namespace server_app.Presentation.Controllers;

[Route("/api/products")]
[ApiController]
public class ProductsController(
    ILogger<ProductsController> logger,
    IProductCategoryRepository productCategoryRepository,
    IDeliveryCompanyRepository deliveryCompanyRepository,
    ISellerRepository sellerRepository,
    ICustomerRepository customerRepository,
    IMapper mapper,
    IRatingService ratingService)
    : ControllerBase
{
    public const string GetIsBoughtRequestHeaderType = "X-Get-Is-Bought";

    public const string IsBoughtHeaderType = "X-Is-Bought";
    public const string IsForOwnerHeaderType = "X-Is-For-Owner";
    public const string CategoriesMaxNumberHeaderType = "X-Categories-Max-Number";
    public const string PurchasedProductsMaxNumberHeaderType = "X-Purchased-Products-Max-Number";


    [HttpGet("recommendation"), ValidationFilter]
    public async Task<IActionResult> GetRecommendation()
    {
        var recommendation = await productCategoryRepository.GetRecommendation();
        return Ok(recommendation);
    }

    [HttpGet("recommendation-by-tag/{tag}"), ValidationFilter]
    public async Task<IActionResult> GetRecommendationByTag([Required] string tag)
    {
        var recommendationByTag = await productCategoryRepository.GetRecommendationByTag(tag);
        return Ok(recommendationByTag);
    }

    [HttpGet("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        //Тут проверяем на владельца, возвращаем владельцу больше данных чем юзеру
        var category = await productCategoryRepository.Get(guid);

        if (category == null)
            return NotFound();

        var isForOwner = IsOwner(category.OwnerId);
        HttpContext?.Response?.Headers?.Append(IsForOwnerHeaderType, isForOwner.ToString());

        if (isForOwner)
            return Ok(mapper.Map<ProductCategoryDtoForOwner>(category));

        if (User.Claims.TryIsCustomer(out var customerGuid))
        {
            await ratingService.SawCategory((Guid)customerGuid, guid);

            var needIsBoughtValue = Request.Headers[GetIsBoughtRequestHeaderType];
            if (!string.IsNullOrEmpty(needIsBoughtValue) && customerGuid != null)
            {
                var isBought =
                    await productCategoryRepository.IsBought(categoryId: category.Id, buyerId: (Guid)customerGuid);
                Response.Headers.Append(IsBoughtHeaderType, isBought.ToString());
            }
        }

        return Ok(mapper.Map<ProductCategoryDtoForViewer>(category));
    }

    [HttpGet("categories"), ValidationFilter]
    public async Task<IActionResult> GetProductCategories(
        [Required, FromQuery, BaseGetQueryValidator]
        GetCategoriesQuery query)
    {
        //Проверяем, если это зашел продавец, то показываем ему его товары
        if (User.Claims.TryIsSeller(out var ownerGuid))
        {
            HttpContext.Response.Headers.Append(IsForOwnerHeaderType, "true");
            var resultForOwner =
                await productCategoryRepository.GetCategoriesByOwner((Guid)ownerGuid, query.From, query.To,
                    query.Search,
                    query.PriceNoMoreThenOrEqual);

            HttpContext.Response.Headers.Append(CategoriesMaxNumberHeaderType, resultForOwner.maxNumber.ToString());
            return Ok(resultForOwner.categories);
        }

        //Если SellerId не валидный то кидаем 400
        if (query.SellerId == null || query.SellerId == Guid.Empty)
            return BadRequest("Your token is invalid");

        //В другом случаи показываем товары продавца с айди = query.SellerId
        var foundSeller = await sellerRepository.Get((Guid)query.SellerId);

        if (foundSeller == null)
            return NotFound("Seller not found");

        HttpContext.Response.Headers.Append(IsForOwnerHeaderType, "false");
        var resultForViewer = await productCategoryRepository.GetCategoriesByViewer(foundSeller.Id, query.From,
            query.To,
            query.Search,
            query.PriceNoMoreThenOrEqual);

        HttpContext.Response.Headers.Append(CategoriesMaxNumberHeaderType, resultForViewer.maxNumber.ToString());
        return Ok(resultForViewer.categories);
    }

    [HttpGet("purchased-products"), Authorize, ValidationFilter]
    public async Task<IActionResult> GetPurchasedProducts([Required, FromQuery] GetPurchasedProductsQuery query)
    {
        if (User.Claims.TryIsCustomer(out var buyerGuid) == false)
            return Forbid();

        var foundCustomer = await customerRepository.Get((Guid)buyerGuid);

        if (foundCustomer == null)
            return NotFound("Customer not found");

        var productsResult =
            await productCategoryRepository.GetPurchasedProducts(foundCustomer.Id, query.From, query.To);

        HttpContext.Response.Headers.Append(PurchasedProductsMaxNumberHeaderType, productsResult.maxNumber.ToString());
        return Ok(productsResult.products);
    }


    [HttpGet("category-name-is-free/{name}"), Authorize, ValidationFilter]
    public async Task<IActionResult> ExistsCategories(string name)
    {
        if (!User.Claims.TryIsSeller(out var sellerGuid))
            return Forbid();

        var nameIsFree = await productCategoryRepository.NameIsFree((Guid)sellerGuid, name);

        return nameIsFree
            ? Ok("Is product categories name free")
            : Conflict("A product category from a seller with that name already exists");
    }

    [HttpPost, Authorize, ValidationFilter]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([Required, FromForm] ProductCategoryCreateQuery query)
    {
        var newUnfinishedCategory = mapper.Map<ProductCategoryCreateDto>(query);

        logger.LogTrace("Images from List<IFormFile>: ");
        foreach (var i in newUnfinishedCategory.Images)
        {
            logger.LogTrace($"New Image {i.FileName}");
            logger.LogTrace($"Mime type: {i.MimeType}");
            logger.LogTrace($"File stream length: {i.FileStream.Length}");
        }
        
        logger.LogInformation($"The User Type: {User.Claims.GetUserType()}");

        if (!User.Claims.TryIsSeller(out var sellerGuid))
            return Forbid();

        Guid ownerGuid = (Guid)sellerGuid;

        var nameIsFree = await productCategoryRepository.NameIsFree(ownerGuid, query.Name);
        if (!nameIsFree)
            return Conflict("Product category with this name already exists");

        var foundSeller = await sellerRepository.Get(ownerGuid);
        var foundCompany = await deliveryCompanyRepository.Get(query.DeliveryCompanyId);

        if (foundCompany == null)
            return NotFound("Delivery company not found");

        newUnfinishedCategory.Owner = foundSeller;
        newUnfinishedCategory.DeliveryCompany = foundCompany;

        var result = await productCategoryRepository.Add(newUnfinishedCategory);
        return result.ResultToIActionResult(logger);
    }

    [HttpPut, Authorize, ValidationFilter]
    public async Task<IActionResult> Update([Required, FromForm] ProductCategoryUpdateQuery query)
    {
        var updatedCategory = await productCategoryRepository.Get(query.Id);

        if (updatedCategory is null)
            return NotFound();

        if (IsOwner(updatedCategory.Owner.Id) == false)
            return Forbid();

        var updateDto = mapper.Map<ProductCategoryUpdateDto>(query);
        await productCategoryRepository.Update(updateDto);

        return Ok();
    }

    [HttpPatch("buy"), Authorize, ValidationFilter]
    public async Task<IActionResult> Buy([Required, FromBody] List<CategoryBuyDto> purchasedCategoriesDtos)
    {
        if (User.Claims.TryIsCustomer(out var buyerId) == false)
            return Forbid();

        var result = await productCategoryRepository.Buy(purchasedCategoriesDtos, (Guid)buyerId);
        return result.ResultToIActionResult();
    }

    [HttpDelete("{guid}"), Authorize, ValidationFilter]
    public async Task<IActionResult> Remove([Required] Guid guid)
    {
        var category = await productCategoryRepository.Get(guid);

        if (category is null)
            return NotFound("Category is not found");

        if (IsOwner(category.Owner.Id))
        {
            await productCategoryRepository.Remove(guid);
            return Ok();
        }

        return Forbid();
    }

    private bool IsOwner(Guid ownerId) => User.Claims.TryIsSeller(out var sellerGuid) && ownerId == sellerGuid;
}