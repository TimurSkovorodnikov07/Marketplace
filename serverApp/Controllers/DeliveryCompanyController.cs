using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/delivery-company")]
public class DeliveryCompanyController(
    ILogger<DeliveryCompanyController> logger,
    DeliveryCompanyService service,
    IMapper mapper)
    : ControllerBase
{
    private readonly ILogger<DeliveryCompanyController> _logger = logger;

    [HttpPost, ValidationFilter, AnonymousOnly]
    public async Task<IActionResult> Create([Required, FromForm] DeliveryCompanyCreateQuery query)
    {
        var phoneNumber = PhoneNumberValueObject.Create(query.PhoneNumber);

        if (!Uri.TryCreate(query.WebSite, new UriCreationOptions(), out Uri? webSite)
            || phoneNumber is null)
            return BadRequest("Not a valid web site and/or phone number");

        var foundCompany = await service.GetByAnyParam(query.Name, webSite, phoneNumber);

        if (foundCompany is not null)
            return Forbid();
        //return Forbid(authenticationSchemes: "A company with such a number, website, or phone number exists.");
        //БЛЯТЬ, я жество обосрался, при этом у меня Rider показывает имена парр., не внимательность короче https://qna.habr.com/q/1372640y

        var newCompany = DeliveryCompanyEntity.Create(query, webSite, phoneNumber);

        if (newCompany is null)
            return BadRequest(); //Вобще такой ситуации не будет, тк есть DataAn. атрибуты на query
        //+ еще проверяю номер и сайт на валидность в начале action, но похуй, пусть будет что ли

        await service.Add(newCompany);

        return Ok();
    }
}