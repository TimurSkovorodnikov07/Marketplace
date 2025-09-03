using System.IdentityModel.Tokens.Jwt;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using server_app.Application.Services;
using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Entities.Users.Seller;
using server_app.Application.Extensions;
using server_app.Application.Options;
using server_app.Application.Repositories;
using server_app.Domain.Model.Dtos;

namespace server_app.Tests.Unit.Services;

public class JwtServiceUnitTests
{
    private readonly IOptions<JwtOptions> _options = Options.Create(new JwtOptions
    {
        Issuer = "issuer",
        AlgorithmForAccessToken = "HS256",
        AccessTokenExpiresMinutes = 15,
        AccessTokenSecretKey = "1234512345123451234512345123451234512345123451234512345",
        AlgorithmForRefreshToken = "RS256",
        RefreshTokenExpiresDays = 25,
        RefreshTokenSecretKey = "secretKeysecsecretK____retKeysecsecretKeysecsecretKeysecsecretKeysecsecretKeysec"
    });
    
    [Fact]
    public async Task GenerateForCustomerTest_ReturnsTokens()
    {
        // Arrange
        var customer = CustomerEntity.Create("HelloWorld", "email@email.com", "passwordhash");
        var id = customer.Id;

        var customerService = A.Fake<ICustomerRepository>();
        A.CallTo(() => customerService.Get(id)).Returns(customer);

        var service = new JwtService(_options, A.Fake<ILogger<JwtService>>(),
            customerService, A.Fake<ISellerRepository>());

        // Act
        var (tokens, isCustomer) = await service.GenerateTokensForCustomerOrSeller(id);

        // Assert
        Assert.NotNull(customer);
        Assert.NotNull(tokens?.RefreshToken);
        Assert.NotNull(tokens?.AccessToken);
        Assert.True(isCustomer);

        Assert.True(IdFromAccessTokenVerify(id, tokens));
        Assert.True(IdFromRefreshTokenVerify(id, tokens));
    }

    [Fact]
    public async Task GenerateForSellerTest_ReturnsTokens()
    {
        // Arrange
        var seller = SellerEntity.Create("HelloWorld", "Description", "selleremail@email.com", "passwordhash");
        var id = seller.Id;

        var customerService = A.Fake<ICustomerRepository>();
        A.CallTo(() => customerService.Get(id)).Returns<CustomerEntity?>(null);
        var sellerService = A.Fake<ISellerRepository>();
        A.CallTo(() => sellerService.Get(id)).Returns(seller);

        var service = new JwtService(_options, A.Fake<ILogger<JwtService>>(),
            customerService, sellerService);

        // Act
        var (tokens, isCustomer) = await service.GenerateTokensForCustomerOrSeller(id);

        // Assert
        Assert.NotNull(seller);
        Assert.NotNull(tokens?.RefreshToken);
        Assert.NotNull(tokens?.AccessToken);
        Assert.False(isCustomer);

        Assert.True(IdFromAccessTokenVerify(id, tokens));
        Assert.True(IdFromRefreshTokenVerify(id, tokens));
    }


    private bool IdFromAccessTokenVerify(Guid id, Tokens tokens)
    {
        var guidStringFromAccess = new JwtSecurityTokenHandler().ReadJwtToken(tokens.AccessToken)
            .Claims.GetUserIdValue();

        return Guid.TryParse(guidStringFromAccess, out var idFromAccess)
               && idFromAccess != Guid.Empty
               && idFromAccess == id;
    }

    private bool IdFromRefreshTokenVerify(Guid id, Tokens tokens)
    {
        var guidStringFromRefresh = new JwtSecurityTokenHandler().ReadJwtToken(tokens.RefreshToken)
            .Claims.GetUserIdValue();

        return Guid.TryParse(guidStringFromRefresh, out var idFromRefresh)
               && idFromRefresh != Guid.Empty
               && idFromRefresh == id;
    }
}