using System.Security.Claims;
using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using server_app.Application.Services.EntitiesServices;
using server_app.Application.Services.EntitiesServices.Interfaces;
using server_app.Application.Services.FileServices;
using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Model.Dtos;
using server_app.Presentation.Controllers;
using server_app.Tests.Intergration;

namespace server_app.Tests.Unit.Controllers;

public class ImagesControllerTests
{
    private const string FirstGuid = "6f6621f8-a116-4516-b65c-88804f46a01c";
    private const string SecondGuid = "34225f6b-b71a-4008-baf8-0de1069f304c";
    private const string ThirdGuid = "e7c3d0bd-f171-4441-a86e-fe37bf87d682";

    [Theory]
    [InlineData(FirstGuid)]
    [InlineData(SecondGuid)]
    [InlineData(ThirdGuid)]
    public async Task GetCategory_AsAnonViewer_ReturnsOk(string guidString)
    {
        //-------
        //Create BuilderClass for makes the productController 


        Assert.True(Guid.TryParse(guidString, out var guid));

        // Arrange
        var emptyCategory = new ProductCategoryEntity { Id = guid, OwnerId = Guid.NewGuid() };
        var emptyDtoForViewer = new ProductCategoryDtoForViewer { Id = guid };

        var service = A.Fake<IProductCategoryService>();
        // BLYAT!!　Корчое [[ЮЗАТЬ ИНТЕРФЕЙСЫ для сервисов]], хотя можно юзать и
        // виртуальные методы которые сама этот фреймворк определит, все же лучше юзать интерфейсы, удобнее.
        A.CallTo(() => service.Get(guid))
            .Returns(Task.FromResult(emptyCategory));


        var mapper = A.Fake<IMapper>();
        A.CallTo(() => mapper.Map<ProductCategoryDtoForViewer>(emptyCategory)).Returns(emptyDtoForViewer);


        var controller = CreateProductsController(
            service, A.Fake<IDeliveryCompanyService>(),
            A.Fake<ISellerService>(), A.Fake<ICustomerService>(),
            mapper, A.Fake<IRatingService>());

        // Act
        var result = await controller.Get(guid);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task GetCategory_NotFound()
    {
        var randomGuid = Guid.NewGuid();

        // Arrange
        var service = A.Fake<IProductCategoryService>();
        A.CallTo(() => service.Get(randomGuid))
            .Returns(Task.FromResult((ProductCategoryEntity?)null));


        var controller = CreateProductsController(
            service, A.Fake<IDeliveryCompanyService>(),
            A.Fake<ISellerService>(), A.Fake<ICustomerService>(),
            A.Fake<IMapper>(), A.Fake<IRatingService>());

        // Act
        var result = await controller.Get(randomGuid);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    private ProductsController CreateProductsController(IProductCategoryService service,
        IDeliveryCompanyService deliveryCompanyService, ISellerService sellerService,
        ICustomerService customerService, IMapper mapper, IRatingService ratingService)
    {
        var controller = new ProductsController(A.Fake<ILogger<ProductsController>>(),
            service, deliveryCompanyService, sellerService,
            customerService, mapper, ratingService);

        var user = new ClaimsPrincipal();
        var context = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = user
            }
        };
        controller.ControllerContext = context;
        
        return controller;
    }
}