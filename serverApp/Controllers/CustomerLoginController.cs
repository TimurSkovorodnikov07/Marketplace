using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("/api/customerlogincontroller")]
public class CustomerLoginController(CustomerService customerService, IServiceProvider provider)
    : BaseLoginController<CustomerEntity, CustomerRegistrationQuery>(provider)
{
    protected override IUserService<CustomerEntity, UserUpdateDto> _userService => customerService;


    [HttpPost, Route("accountcreate"), AnonymousOnly, ValidationFilter]
    public override async Task<IActionResult> AccountCreate([FromForm] CustomerRegistrationQuery dto)
    {
        var existingUser = await GetExistingUser(dto.Email, dto.Password);

        if (existingUser == null)
        {
            var passwordHash = _hasher.Hashing(dto.Password);
            var newUser = CustomerEntity.Create(dto, passwordHash);

            if (newUser == null)
                return BadRequest("Customer not valid");

            await customerService.Add(newUser);
            _logger.LogDebug("Created user: {0}", newUser.Name);

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

        return Conflict("A user with such an email already exists");
    }

    [HttpPatch, Route("addcard"), Authorize, ValidationFilter]
    public async Task<IActionResult> AddCreditCard([Required] CreditCardAddQuery query)
    {
        if (!TryIsBuyer(out var buyerGuid))
            return Forbid();

        var newCreditCard = CreditCardEntity.Create(query.Number,
            (Guid)buyerGuid, query.Type, query.Many);

        if (newCreditCard == null)
            return BadRequest("No valid credit card");

        //Проверять выполнился ли AddCard успешно не зачем, тк и так и так существующего юзера айдишник
        await customerService.AddCard(newCreditCard);
        return Ok();
    }

    private bool TryIsBuyer(out Guid? guid)
    {
        if (User.Claims.TryGetCustomerIdValue(out string? buyerIdString)
            && Guid.TryParse(buyerIdString, out Guid buyerId))
        {
            guid = buyerId;
            return true;
        }

        guid = null;
        return false;
    }
}