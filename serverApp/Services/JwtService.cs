using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
public class JwtService<UserT>(IOptions<JwtOptions> options, ILogger<JwtService<UserEntity>> logger)
    where UserT : UserEntity
{
    private readonly JwtOptions _options = options.Value;
    private readonly ILogger<JwtService<UserEntity>> _logger = logger;

    public string AccessTokenCreate(UserT user)
    {
        var signingCredentials = new SigningCredentials(
           _options.GetAccessSymmetricSecurityKey(), _options.AlgorithmForAccessToken);

        var claims = new List<Claim>
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("userName", user.Name.ToString()),
            new Claim("userEmail", user.Email.ToString()),
            new Claim("userType", GetUserType(user.GetType())),
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenExpiresMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string RefreshTokenCreate(UserT user)
    {
        var signingCredentials = new SigningCredentials(
           _options.GetRefreshAsymmetricSecurityKey(), _options.AlgorithmForRefreshToken);

        var claims = new Claim[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim("userName", user.Name.ToString()),
            new Claim("userEmail", user.Email.ToString()),
            new Claim("userType", GetUserType(user.GetType())),
        };


        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddDays(_options.RefreshTokenExpiresDays),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public string GetUserType(Type userType) => userType ==  typeof(CustomerEntity) ? "customer" : "seller"; 
    public bool RefreshTokenIsValid(string refreshToken, Guid userId)
    {
        var token = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
        
        return token.ValidTo > DateTime.UtcNow &
            token.Claims?.First(c => c.Type == "userId")?.Value == userId.ToString();
    }
}