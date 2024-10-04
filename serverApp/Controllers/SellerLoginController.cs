using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/seller-login")]
public class SellerLoginController(SellerService sellerService, IServiceProvider provider)
    : BaseLoginController<SellerEntity, SellerRegistrationQuery>(provider)
{
    protected override IUserService<SellerEntity, UserUpdateDto> _userService => sellerService;


    [HttpPost, Route("accountcreate"), AnonymousOnly, ValidationFilter]
    public override async Task<IActionResult> AccountCreate([FromForm] SellerRegistrationQuery dto)
    {
        //Вобще создание акк различаеться у продовца и пок потому стоит их реализовать у каждого по разному
        var existingUser = await GetExistingUser(dto.Email, dto.Password);

        if (existingUser == null)
        {
            var passwordHash = _hasher.Hashing(dto.Password);
            var newUser = SellerEntity.Create(dto, passwordHash);
            
            if (newUser == null)
                return BadRequest();

            await _userService.Add(newUser);
            _logger.LogDebug("Created Seller: {0}", newUser.Name);

            try
            {
                await _emailVerify.CodeSend(newUser.Id, newUser.Email);
            }
            catch (SmtpException e)
            {
                return StatusCode(((int)e.StatusCode), e.Message);
            }

            return Ok(new
            {
                UserId = newUser.Id.ToString(),
                CodeDiedAfterSeconds = _verifierCodeOptions.DiedAfterSeconds.ToString(),
                CodeLength = _verifierCodeOptions.Length.ToString()
            });
        }

        return Conflict("A Seller with such an email already exists");
    }
}