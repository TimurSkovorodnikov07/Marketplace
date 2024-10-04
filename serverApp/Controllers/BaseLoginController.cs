using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

public abstract class BaseLoginController<UserT, RegistrationQueryT>(IServiceProvider serviceProvider) : ControllerBase
    where UserT : UserEntity
    where RegistrationQueryT : UserRegistrationQuery
{
    protected readonly IHasher _hasher = serviceProvider.GetService<IHasher>();
    protected readonly IHashVerify _hashVerify = serviceProvider.GetService<IHashVerify>();

    protected readonly ILogger<BaseLoginController<UserEntity, UserRegistrationQuery>> _logger =
        serviceProvider.GetService<ILogger<BaseLoginController<UserEntity, UserRegistrationQuery>>>();

    protected readonly IEmailVerify _emailVerify = serviceProvider.GetService<IEmailVerify>();
    protected readonly JwtService<UserT> _jwtService = serviceProvider.GetService<JwtService<UserT>>();

    protected readonly VerfiyCodeOptions _verifierCodeOptions =
        serviceProvider.GetService<IOptions<VerfiyCodeOptions>>().Value;

    protected readonly RefreshTokenService _refreshService = serviceProvider.GetService<RefreshTokenService>();


    protected abstract IUserService<UserT, UserUpdateDto> _userService { get; }
    public abstract Task<IActionResult> AccountCreate([FromForm] RegistrationQueryT dto);


    [HttpPost, Route("login"), AnonymousOnly, ValidationFilter]
    public virtual async Task<IActionResult> Login([FromForm] UserLoginQuery dto)
    {
        var confirmedUser = await _userService.GetConfirmedUser(dto.Email);

        if (confirmedUser is null)
            return NotFound("The User was not found with such a email");

        if (!_hashVerify.Verify(dto.Password, confirmedUser.PasswordHash))
            return BadRequest("Invalid password");

        try
        {
            await _emailVerify.CodeSend(confirmedUser.Id, confirmedUser.Email);
        }
        catch (SmtpException e)
        {
            return StatusCode(((int)e.StatusCode), e.Message);
        }

        return Ok(new
        {
            UserId = confirmedUser.Id.ToString(),
            CodeDiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
            CodeLength = _verifierCodeOptions.Length.ToString()
        });
    }

    [HttpGet, Route("userinfo"), Authorize, ValidationFilter]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User.Claims.GetUserIdValue();

        if (string.IsNullOrEmpty(userId))
            return BadRequest();

        var user = await _userService.Get(new Guid(userId));

        return Ok(user != null
            ? new UserDto { Id = user.Id.ToString(), Email = user.Email, Name = user.Name }
            : null);
    }

    [HttpPut, Route("coderesend/{userId}"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> CodeResend(Guid userId)
    {
        var user = await _userService.Get(userId);

        if (user is null)
            return NotFound("User not found");

        try
        {
            await _emailVerify.Resend(userId, user.Email);
        }
        catch (SmtpException e)
        {
            return StatusCode(((int)e.StatusCode), e.Message);
        }

        return Ok(new
        {
            CodeDiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
            CodeLength = _verifierCodeOptions.Length.ToString()
        });
    }

    [HttpPut, Route("tokensupdate"), AnonymousOnly, ValidationFilter]
    public virtual async Task<IActionResult> TokensUpdate([FromBody] TokensUpdateQuery query)
    {
        var oldToken = await _refreshService.GetByUserId(query.UserId);

        if (oldToken is null)
            return NotFound("Token not found");
        else if (_hashVerify.Verify(query.OldRefreshToken, oldToken.TokenHash) == false)
            return BadRequest("The wrong token");

        var user = await _userService.Get(oldToken.UserId);

        if (user is null)
            return NotFound("User not found");

        var tokens = TokensCreate(user);
        var newRefreshToken = RefreshTokenEntity.Create(oldToken.UserId, _hasher.Hashing(tokens.RefreshToken));

        await _refreshService.Update(newRefreshToken);
        return Ok(tokens);
    }

    [HttpPost, Route("emailverify"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> EmailVerify([FromBody] EmailVerifyQuery query)
    {
        var foundUser = await _userService.Get(query.UserId);

        if (foundUser is null)
            return NotFound("Seller not found");

        var verifyRes = await _emailVerify.CodeVerify(query.UserId, query.Code);

        if (verifyRes)
        {
            var findUserId = foundUser.Id;

            await _userService.EmailVerUpdate(findUserId);
            var tokens = TokensCreate(foundUser);

            var newRefreshToken = RefreshTokenEntity.Create(findUserId, _hasher.Hashing(tokens.RefreshToken));
            await _refreshService.AddOrUpdate(newRefreshToken);

            return Ok(tokens);
        }

        return BadRequest();
    }

    [NonAction]
    protected Tokens TokensCreate(UserT user)
    {
        var accessToken = _jwtService.AccessTokenCreate(user);
        var refreshToken = _jwtService.RefreshTokenCreate(user);

        return new(accessToken, refreshToken);
    }

    [NonAction]
    protected async Task<UserT?> GetExistingUser(string email, string password) =>
        await _userService.GetExistingUser(email, _hasher.Hashing(password));
}