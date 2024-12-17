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

    [HttpGet("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        var company = await service.Get(guid);

        return company == null
            ? NotFound("Company not found")
            : Ok(mapper.Map<DeliveryCompanyForViewerDto>(company));
    }

    [HttpGet("companies"), ValidationFilter]
    public async Task<IActionResult> GetCompanies([Required, FromQuery] GetDeliveryCompanyQuery query)
    {
        var companies =
            string.IsNullOrEmpty(query.CompanyName)
                ? service.GetAllCompanies()
                : service.SearchCompaniesByName(query.CompanyName);

        return Ok(companies);
    }

    [HttpPost, ValidationFilter]
    public async Task<IActionResult> Create([Required, FromForm] DeliveryCompanyCreateQuery query)
    {
        var phoneNumber = PhoneNumberValueObject.Create(query.PhoneNumber);

        if (!Uri.TryCreate(query.WebSite, new UriCreationOptions(), out Uri? webSite)
            || phoneNumber is null)
            return BadRequest("Not a valid web site and/or phone number");

        var foundCompany = await service.GetByAnyParam(query.Name, webSite, phoneNumber);

        if (foundCompany is not null)
            return BadRequest("A company with that name, number, or website already exists");

        var newCompany = DeliveryCompanyEntity.Create(query, webSite, phoneNumber);

        if (newCompany is null)
            return BadRequest(); //Вобще такой ситуации не будет, тк есть DataAn. атрибуты на query
        //+ еще проверяю номер и сайт на валидность в начале action, но похуй, пусть будет что ли

        await service.Add(newCompany);
        return Ok();
    }
}