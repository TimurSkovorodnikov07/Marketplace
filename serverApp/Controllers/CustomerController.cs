using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

[ApiController, Route("/api/customer-controller")]
public class CustomerController(
    CustomerService customerService,
    IHasher hasher,
    ILogger<CustomerController> logger,
    IEmailVerify emailVerify,
    IOptions<VerfiyCodeOptions> verifyCodeOptions) : ControllerBase
{
    private readonly VerfiyCodeOptions _verifyCodeOptions = verifyCodeOptions.Value;

    [HttpPost, Route("accountcreate"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> AccountCreate([FromForm] CustomerRegistrationQuery dto)
    {
        var confirmedUser = await customerService.GetConfirmedUser(dto.Email);
        var existingUser = await customerService.GetExistingUser(dto.Email, dto.Password);

        if (existingUser != null || confirmedUser != null)
            return Conflict("A customer with such an email already exists.");
        
        var passwordHash = hasher.Hashing(dto.Password);
        var newUser = CustomerEntity.Create(dto, passwordHash);

        if (newUser == null)
            return BadRequest("Customer not valid");

        await customerService.Add(newUser);
        logger.LogDebug("Created user: {0}", newUser.Name);

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

    [HttpPatch, Route("addcard"), Authorize, ValidationFilter]
    public async Task<IActionResult> AddCreditCard([Required, CreditCardAddQueryValidation] CreditCardAddQuery query)
    {
        logger.LogCritical("NIGGERSSS!!!!");
        
        if (Enum.TryParse(query.Type, true,
                out CreditCardType cardType) == false)
            return BadRequest("Invalid card type");

        if (!User.Claims.TryIsCustomer(out var customerId))
            return Forbid();

        var owner = await customerService.Get((Guid)customerId);

        if (owner == null)
        {
            logger.LogCritical("Если разраб запихал в токен НЕ существующего юзера он " +
                               "инвалид(если удалил просто его, а тот пытаеться зайти ну ок тогда)");
            return NotFound("Customer not found");
        }

        var numberHash = hasher.Hashing(query.Number);
        var newCreditCard = CreditCardEntity.Create(numberHash, owner,
            cardType, query.Many);

        if (newCreditCard == null)
            return BadRequest("Invalid credit card");

        var addResult = await customerService.AddCard(newCreditCard, owner.Id);
        return addResult.ActionResult;
    }
}