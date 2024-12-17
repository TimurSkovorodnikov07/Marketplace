using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtService(
    IOptions<JwtOptions> options,
    ILogger<JwtService> logger,
    CustomerService customerService,
    SellerService sellerService)
{
    public const string UserIdClaimType = "userId";
    public const string UserNameClaimType = "userName";
    public const string UserEmailClaimType = "userEmail";
    public const string UserTypeClaimType = "userType";

    private readonly JwtOptions _options = options.Value;
    private readonly ILogger<JwtService> _logger = logger;

    private List<Claim> GetClaims<UserT>(UserT user) where UserT : UserEntity
    {
        return new List<Claim>
        {
            new Claim(UserIdClaimType, user.Id.ToString()),
            new Claim(UserNameClaimType, user.Name.ToString()),
            new Claim(UserEmailClaimType, user.Email.ToString()),
            new Claim(UserTypeClaimType, GetUserType(typeof(UserT))),
        };
    }

    public string AccessTokenCreate<UserT>(UserT user) where UserT : UserEntity

    {
        var signingCredentials = new SigningCredentials(
            _options.GetAccessSymmetricSecurityKey(), _options.AlgorithmForAccessToken);

        var claims = GetClaims(user);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenExpiresMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string RefreshTokenCreate<UserT>(UserT user) where UserT : UserEntity
    {
        var signingCredentials = new SigningCredentials(
            _options.GetRefreshAsymmetricSecurityKey(), _options.AlgorithmForRefreshToken);

        var claims = GetClaims(user);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(_options.RefreshTokenExpiresDays),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GetUserType(Type userType) => userType == typeof(CustomerEntity) ? "customer" : "seller";


    //Generate Tokens
    public async Task<(Tokens? tokens, bool isCustomer)> GenerateTokensForCustomerOrSeller(Guid guid)
    {
        var customer = await customerService.Get(guid);

        if (customer is not null)
            return (TokensCreateForCustomer(customer), true);

        var seller = await sellerService.Get(guid);

        return seller is null 
            ? (null, false)
            : (TokensCreateForSeller(seller), false);
    }

    private Tokens TokensCreateForSeller(SellerEntity seller)
    {
        var accessToken = AccessTokenCreate(seller);
        var refreshToken = RefreshTokenCreate(seller);

        return new(accessToken, refreshToken);
    }

    private Tokens TokensCreateForCustomer(CustomerEntity customer)
    {
        var accessToken = AccessTokenCreate(customer);
        var refreshToken = RefreshTokenCreate(customer);

        return new(accessToken, refreshToken);
    }
}