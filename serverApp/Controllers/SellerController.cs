using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[ApiController, Route("/api/seller-controller")]
public class SellerController(
    SellerService sellerService,
    IHasher hasher,
    ILogger<CustomerController> logger,
    IEmailVerify emailVerify,
    IOptions<VerfiyCodeOptions> verifyCodeOptions,
    IMapper mapper) : ControllerBase
{
    private readonly VerfiyCodeOptions _verifyCodeOptions = verifyCodeOptions.Value;

    public const string GetForOwnerHeaderType = "X-Get-For-Owner";

    [HttpGet("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        var foundSeller = await sellerService.Get(guid);

        if (foundSeller is null)
            return NotFound();

        var isForOwner = User.Claims.TryIsSeller(out var sellerGuid) && sellerGuid == guid;
        
        object responseData = isForOwner 
            ? mapper.Map<SellerDtoForOwner>(foundSeller)
            : mapper.Map<SellerDtoForViewer>(foundSeller);

        Response.Headers.Append(GetForOwnerHeaderType, isForOwner.ToString().ToLower());
        return Ok(responseData);
    }


    [HttpPost, Route("accountcreate"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> AccountCreate([FromForm] SellerRegistrationQuery dto)
    {
        var confirmedUser = await sellerService.GetConfirmedUser(dto.Email);
        var existingUser = await sellerService.GetExistingUser(dto.Email, dto.Password);

        if (existingUser != null || confirmedUser != null)
            return Conflict("A seller with such an email already exists.");

        var passwordHash = hasher.Hashing(dto.Password);
        var newUser = SellerEntity.Create(dto, passwordHash);

        if (newUser == null)
            return BadRequest();

        await sellerService.Add(newUser);
        logger.LogDebug("Created Seller: {0}", newUser.Name);

        try
        {
            await emailVerify.CodeSend(newUser.Id, newUser.Email);
        }
        catch (SmtpException e)
        {
            return StatusCode(((int)e.StatusCode), e.Message);
        }

        return Ok(new
        {
            UserId = newUser.Id.ToString(),
            CodeDiedAfterSeconds = _verifyCodeOptions.DiedAfterSeconds.ToString(),
            CodeLength = _verifyCodeOptions.Length.ToString()
        });

    }
}