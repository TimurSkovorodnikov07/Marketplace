using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("/api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    public ProductsController(ILogger<ProductsController> logger,
        ProductCategoryService productCategoryService, DeliveryCompanyService deliveryCompanyService,
        SellerService sellerService, IMapper mapper)
    {
        _mapper = mapper;
        _productCategoryService = productCategoryService;
        _deliveryCompanyService = deliveryCompanyService;
        _sellerService = sellerService;
        _logger = logger;
    }

    private readonly ProductCategoryService _productCategoryService;
    private readonly DeliveryCompanyService _deliveryCompanyService;
    private readonly SellerService _sellerService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductsController> _logger;

    [HttpGet("{guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        //Тут проверяем на владельца, возвращаем владельцу больше данных чем юзеру
        var category = await _productCategoryService.Get(guid);

        if (category == null)
            return NotFound();

        return IsOwner(category.Owner.Id)
            ? Ok(_mapper.Map<ProductCategoryDtoForOwner>(category))
            : Ok(_mapper.Map<ProductCategoryDtoForViewer>(category));
    }

    [HttpGet, Authorize, ValidationFilter]
    public async Task<IActionResult> GetProductCategories(GetCategoriesQuery query)
    {
        //Проверяем, если это зашел продавец, то показываем ему его товары
        if (User.Claims.TryIsSeller(out var ownerGuid))
            return Ok(await _productCategoryService.GetCategoriesByOwner((Guid)ownerGuid, query));

        //В другом случаи показываем товары продавца с айди = query.SellerId
        if (query.SellerId != null && query.SellerId != Guid.Empty)
            return Ok(await _productCategoryService.GetCategoriesByViewer((Guid)query.SellerId, query));

        //Если SellerId не валидный то кидаем 400
        return BadRequest();
    }


    [HttpPost, Authorize, ValidationFilter]
    public async Task<IActionResult> Create([Required, FromForm] ProductCategoryCreateQuery query)
    {
        var newUnfinishedCategory = _mapper.Map<ProductCategoryCreateDto>(query);

        if (!User.Claims.TryIsSeller(out var sellerGuid))
            return Forbid();
        Guid ownerGuid = (Guid)sellerGuid;

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

        if (IsOwner(updatedCategory.Owner.Id))
        {
            var updateDto = _mapper.Map<ProductCategoryUpdateDto>(query);
            await _productCategoryService.Update(updateDto);

            return Ok();
        }

        return Forbid();
    }

    [HttpPatch, Authorize, ValidationFilter]
    public async Task<IActionResult> Buy(ProductBuyQuery query)
    {
        return User.Claims.TryIsCustomer(out var buyerId)
            ? (await _productCategoryService.Buy(query, (Guid)buyerId)).ActionResult
            : Forbid();
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