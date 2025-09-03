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
using server_app.Domain.Entities.Users.CreditCard;
using server_app.Domain.Entities.Users.Customer;
using server_app.Presentation.Filters;
using server_app.Presentation.Filters.ValidatorAttributes;
using server_app.Presentation.ModelQueries;

namespace server_app.Presentation.Controllers.UserControllers;

[ApiController, Route("/api/customer-controller")]
public class CustomerController(
    ICustomerRepository customerRepository,
    IHasher hasher,
    ILogger<CustomerController> logger,
    IEmailRepository emailRepository,
    IOptions<VerfiyCodeOptions> verifyCodeOptions) : ControllerBase
{
    private readonly VerfiyCodeOptions _verifyCodeOptions = verifyCodeOptions.Value;

    [HttpPost, Route("accountcreate"), AnonymousOnly, ValidationFilter]
    public async Task<IActionResult> AccountCreate([FromForm] CustomerRegistrationQuery dto)
    {
        var confirmedUser = await customerRepository.GetConfirmedUser(dto.Email);
        var existingUser = await customerRepository.GetExistingUser(dto.Email, dto.Password);

        if (existingUser != null || confirmedUser != null)
            return Conflict("A customer with such an email already exists.");

        var passwordHash = hasher.Hashing(dto.Password);
        var newUser = CustomerEntity.Create(
            name: dto.Name,
            email: dto.Email,
            passwordHash: passwordHash);

        if (newUser == null)
            return BadRequest("Customer not valid");

        await customerRepository.Add(newUser);
        logger.LogDebug("Created user: {0}", newUser.Name);
        
        try
        {
            await emailRepository.CodeSend(newUser.Id, newUser.Email);
        }
        catch (SmtpException e)
        {
            return StatusCode(((int)e.StatusCode), e.Message);
        }

        return Ok(new
        {
            UserId = newUser.Id,
            CodeDiedAfterSeconds = _verifyCodeOptions.DiedAfterSeconds,
            CodeLength = _verifyCodeOptions.Length
        });
    }

    [HttpPatch, Route("addcard"), Authorize, ValidationFilter]
    public async Task<IActionResult> AddCreditCard([Required, CreditCardAddQueryValidation] CreditCardAddQuery query)
    {
        if (Enum.TryParse(query.Type, true,
                out CreditCardType cardType) == false)
            return BadRequest("Invalid card type");

        if (!User.Claims.TryIsCustomer(out var customerId))
            return Forbid();

        var owner = await customerRepository.Get((Guid)customerId);

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

        await customerRepository.AddCard(newCreditCard, owner.Id);
        return Ok();
    }
}