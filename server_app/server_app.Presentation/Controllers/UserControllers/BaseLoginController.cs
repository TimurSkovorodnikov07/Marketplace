using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using server_app.Application.Abstractions.EmailSend;
using server_app.Application.Abstractions.Hashing;
using server_app.Application.Extensions;
using server_app.Application.Options;
using server_app.Application.Repositories;
using server_app.Application.Services;
using server_app.Domain.Users.Tokens;
using server_app.Presentation.Filters;
using server_app.Presentation.ModelQueries;

namespace server_app.Presentation.Controllers.UserControllers;

[ApiController, Route("/api/baselogincontroller")]
public class BaseLoginController(
    IUserRepository userRepository,
    IHasher hasher,
    IHashVerify hashVerify,
    ILogger<BaseLoginController> logger,
    IEmailRepository emailRepository,
    IOptions<VerfiyCodeOptions> verifyCodeOptions,
    IRefreshTokenRepository refreshTokenRepository,
    JwtService jwtService) : ControllerBase
{
    public const string AccountIsConfirmedHeaderType = "X-Account-Is-Confirmed";
    private readonly VerfiyCodeOptions _verifyCodeOptions = verifyCodeOptions.Value;

    [HttpPost, Route("login"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> Login([FromForm, Required] UserLoginQuery query)
    {
        //Confirmed - подвердил почту
        //Existed - созданный, не обез что подверж
        var confirmedUser = await userRepository.GetConfirmedUser(query.Email);
        if (confirmedUser == null)
        {
            Response.Headers.Append(AccountIsConfirmedHeaderType, "false");
            var existingUser = await userRepository.GetExistingUser(query.Email, query.Password);

            return existingUser == null
                ? NotFound("User not found")
                : await CodeResend(existingUser.Id);
        }

        Response.Headers.Append(AccountIsConfirmedHeaderType, "true");

        if (!hashVerify.Verify(query.Password, confirmedUser.PasswordHash))
            return BadRequest("Invalid password");

        return await AccountConfirmed(confirmedUser.Id, true);
    }

    [HttpGet, Route("userinfo"), Authorize, ValidationFilter]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User.Claims.GetUserIdValue();

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return BadRequest("Invalid user id, check your token");

        var tuple = await userRepository.GetWithInfoWhoIt(userGuid);
        var (customer, seller) = tuple;
        var isCustomer = customer != null;

        if (customer == null && seller == null)
            return NotFound("User was not found");

        object responseData =
            isCustomer
                ? new
                {
                    Id = customer.Id.ToString(),
                    Email = customer.Email,
                    Name = customer.Name,
                    isCustomer = isCustomer,
                }
                : new
                {
                    Id = seller.Id.ToString(),
                    Email = seller.Email,
                    Name = seller.Name,
                    Description = seller.Description,
                    isCustomer = isCustomer,
                };
        return Ok(responseData);
    }

    [HttpPut, Route("coderesend/{userId}"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> CodeResend([Required] Guid userId)
    {
        var (user, isSeller) = await userRepository.Get(userId);

        if (user is null)
            return NotFound("User not found");

        try
        {
            await emailRepository.Resend(userId, user.Email);
        }
        catch (SmtpException e)
        {
            return StatusCode(((int)e.StatusCode), e.Message);
        }

        return Ok(new
        {
            UserId = user.Id,
            CodeDiedAfterSeconds = _verifyCodeOptions.DiedAfterSeconds,
            CodeLength = _verifyCodeOptions.Length
        });
    }

    [HttpPut, Route("tokensupdate"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> TokensUpdate([FromBody] TokensUpdateQuery query)
    {
        var oldToken = await refreshTokenRepository.GetByUserId(query.UserId);

        if (oldToken is null)
            return NotFound("Token not found");
        if (hashVerify.Verify(query.OldRefreshToken, oldToken.TokenHash) == false)
            return BadRequest("The wrong token");

        return await AccountConfirmed(query.UserId, true);
    }

    [HttpPost, Route("emailverify"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> EmailVerify([FromBody] EmailVerifyQuery query)
    {
        var verifyRes = await emailRepository.CodeVerify(query.UserId, query.Code);

        if (verifyRes)
            return await AccountConfirmed(query.UserId, false);

        return BadRequest("Code is invalid");
    }

    [NonAction]
    private async Task<IActionResult> AccountConfirmed(Guid id, bool exectlyUpdate)
    {
        var (tokens, isCustomer) = await jwtService.GenerateTokensForCustomerOrSeller(id);

        if (tokens is null)
            return NotFound("User not found");

        var newRefreshToken = RefreshTokenEntity.Create(id, hasher.Hashing(tokens.RefreshToken));

        if (exectlyUpdate)
            await refreshTokenRepository.Update(newRefreshToken);
        else
            await refreshTokenRepository.AddOrUpdate(newRefreshToken);

        return Ok(new
        {
            RefreshToken = tokens.RefreshToken,
            AccessToken = tokens.AccessToken,
            UserId = id.ToString(),
            IsCustomer = isCustomer
        });
    }
}