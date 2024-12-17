using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[Route("/api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    public ProductsController(ILogger<ProductsController> logger,
        ProductCategoryService productCategoryService, DeliveryCompanyService deliveryCompanyService,
        SellerService sellerService, CustomerService customerService,
        IMapper mapper, RatingService ratingService)
    {
        _mapper = mapper;
        _productCategoryService = productCategoryService;
        _deliveryCompanyService = deliveryCompanyService;
        _sellerService = sellerService;
        _customerService = customerService;
        _ratingService = ratingService;
        _logger = logger;
    }

    private readonly ProductCategoryService _productCategoryService;
    private readonly DeliveryCompanyService _deliveryCompanyService;
    private readonly SellerService _sellerService;
    private readonly CustomerService _customerService;
    private readonly RatingService _ratingService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductsController> _logger;

    public const string GetIsBoughtRequestHeaderType = "X-Get-Is-Bought";

    public const string IsBoughtHeaderType = "X-Is-Bought";
    public const string IsForOwnerHeaderType = "X-Is-For-Owner";
    public const string CategoriesMaxNumberHeaderType = "X-Categories-Max-Number";
    public const string PurchasedProductsMaxNumberHeaderType = "X-Purchased-Products-Max-Number";


    [HttpGet("recommendation"), ValidationFilter]
    public async Task<IActionResult> GetRecommendation()
    {
        var recommendation = await _productCategoryService.GetRecommendation();
        return Ok(recommendation);
    }

    [HttpGet("recommendation-by-tag/{tag}"), ValidationFilter]
    public async Task<IActionResult> GetRecommendationByTag([Required] string tag)
    {
        var recommendationByTag = await _productCategoryService.GetRecommendationByTag(tag);
        return Ok(recommendationByTag);
    }

    [HttpGet("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        //Тут проверяем на владельца, возвращаем владельцу больше данных чем юзеру
        var category = await _productCategoryService.Get(guid);

        if (category == null)
            return NotFound();

        var isForOwner = IsOwner(category.Owner.Id);
        HttpContext.Response.Headers.Append(IsForOwnerHeaderType, isForOwner.ToString());

        if (isForOwner)
            return Ok(_mapper.Map<ProductCategoryDtoForOwner>(category));

        if (User.Claims.TryIsCustomer(out var customerGuid))
            await _ratingService.SawCategory((Guid)customerGuid, guid);

        var needIsBoughtValue = Request.Headers[GetIsBoughtRequestHeaderType];
        if (!string.IsNullOrEmpty(needIsBoughtValue) && customerGuid != null)
        {
            var isBought = await _productCategoryService.IsBought(categoryId: category.Id, buyerId: (Guid)customerGuid);
            Response.Headers.Append(IsBoughtHeaderType, isBought.ToString());
        }

        return Ok(_mapper.Map<ProductCategoryDtoForViewer>(category));
    }

    [HttpGet("categories"), ValidationFilter]
    public async Task<IActionResult> GetProductCategories([Required, FromQuery] GetCategoriesQuery query)
    {
        //Проверяем, если это зашел продавец, то показываем ему его товары
        if (User.Claims.TryIsSeller(out var ownerGuid))
        {
            HttpContext.Response.Headers.Append(IsForOwnerHeaderType, "true");
            var resultForOwner =
                await _productCategoryService.GetCategoriesByOwner((Guid)ownerGuid, query);

            HttpContext.Response.Headers.Append(CategoriesMaxNumberHeaderType, resultForOwner.maxNumber.ToString());
            return Ok(resultForOwner.categories);
        }

        //Если SellerId не валидный то кидаем 400
        if (query.SellerId == null || query.SellerId == Guid.Empty)
            return BadRequest("Your token is invalid");

        //В другом случаи показываем товары продавца с айди = query.SellerId
        var foundSeller = await _sellerService.Get((Guid)query.SellerId);

        if (foundSeller == null)
            return NotFound("Seller not found");

        HttpContext.Response.Headers.Append(IsForOwnerHeaderType, "false");
        var resultForViewer = await _productCategoryService.GetCategoriesByViewer(foundSeller.Id, query);

        HttpContext.Response.Headers.Append(CategoriesMaxNumberHeaderType, resultForViewer.maxNumber.ToString());
        return Ok(resultForViewer.categories);
    }

    [HttpGet("purchased-products"), Authorize, ValidationFilter]
    public async Task<IActionResult> GetPurchasedProducts([Required, FromQuery] GetPurchasedProductsQuery query)
    {
        if (User.Claims.TryIsCustomer(out var buyerGuid) == false)
            return Forbid();

        var foundCustomer = await _customerService.Get((Guid)buyerGuid);

        if (foundCustomer == null)
            return NotFound("Customer not found");

        var productsResult =
            await _productCategoryService.GetPurchasedProducts(foundCustomer.Id, query.From, query.To);

        HttpContext.Response.Headers.Append(PurchasedProductsMaxNumberHeaderType, productsResult.maxNumber.ToString());
        return Ok(productsResult.products);
    }


    [HttpGet("category-name-is-free/{name}"), Authorize, ValidationFilter]
    public async Task<IActionResult> ExistsCategories(string name)
    {
        if (!User.Claims.TryIsSeller(out var sellerGuid))
            return Forbid();

        var nameIsFree = await _productCategoryService.NameIsFree((Guid)sellerGuid, name);

        return nameIsFree
            ? Ok("Is product categories name free")
            : Conflict("A product category from a seller with that name already exists");
    }

    [HttpPost, Authorize, ValidationFilter]
    public async Task<IActionResult> Create([Required, FromForm] ProductCategoryCreateQuery query)
    {
        var newUnfinishedCategory = _mapper.Map<ProductCategoryCreateDto>(query);

        if (!User.Claims.TryIsSeller(out var sellerGuid))
            return Forbid();
        //return Forbid(authenticationSchemes: "Your not seller");
        //БЛЯТЬ, я жество обосрался, при этом у меня Rider показывает имена парр., не внимательность короче https://qna.habr.com/q/1372640y
        Guid ownerGuid = (Guid)sellerGuid;

        var nameIsFree = await _productCategoryService.NameIsFree(ownerGuid, query.Name);
        if (!nameIsFree)
            return Conflict("Product category with this name already exists");

        var foundSeller = await _sellerService.Get(ownerGuid);
        var foundCompany = await _deliveryCompanyService.Get(query.DeliveryCompanyId);

        if (foundCompany == null)
            return NotFound("Delivery company not found");
        //Проверять наличие юзера в бд не нужно тк если разраб НЕ долбоеб он закинет в jwt token СУЩЕСТВУЮЩЕГО продовца

        newUnfinishedCategory.Owner = foundSeller;
        newUnfinishedCategory.DeliveryCompany = foundCompany;

        return (await _productCategoryService.Add(newUnfinishedCategory)).ActionResult;
    }

    [HttpPut, Authorize, ValidationFilter]
    public async Task<IActionResult> Update([Required, FromForm] ProductCategoryUpdateQuery query)
    {
        var updatedCategory = await _productCategoryService.Get(query.Id);

        if (updatedCategory is null)
            return NotFound();

        if (IsOwner(updatedCategory.Owner.Id) == false)
            return Forbid();

        var updateDto = _mapper.Map<ProductCategoryUpdateDto>(query);
        await _productCategoryService.Update(updateDto);

        return Ok();
    }

    [HttpPatch("buy"), Authorize, ValidationFilter]
    public async Task<IActionResult> Buy([Required, FromBody] List<CategoryBuyDto> purchasedCategoriesDtos)
    {
        if (User.Claims.TryIsCustomer(out var buyerId) == false)
            return Forbid();

        var result = await _productCategoryService.Buy(purchasedCategoriesDtos, (Guid)buyerId);
        return result.ActionResult;
    }

    [HttpDelete("{guid}"), Authorize, ValidationFilter]
    public async Task<IActionResult> Remove([Required] Guid guid)
    {
        var category = await _productCategoryService.Get(guid);

        if (category is null)
            return NotFound("Category is not found");

        if (IsOwner(category.Owner.Id))
        {
            await _productCategoryService.Remove(guid);
            return Ok();
        }

        return Forbid();
    }

    private bool IsOwner(Guid ownerId) => User.Claims.TryIsSeller(out var sellerGuid) && ownerId == sellerGuid;
}