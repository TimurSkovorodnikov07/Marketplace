using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using server_app.Application.Repositories;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Model.Dtos;
using server_app.Presentation.Filters;
using server_app.Presentation.ModelQueries;

namespace server_app.Presentation.Controllers;

[ApiController]
[Route("/api/delivery-company")]
public class DeliveryCompanyController(
    ILogger<DeliveryCompanyController> logger,
    IDeliveryCompanyRepository repository,
    IMapper mapper)
    : ControllerBase
{
    [HttpGet("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        var company = await repository.Get(guid);

        return company == null
            ? NotFound("Company not found")
            : Ok(mapper.Map<DeliveryCompanyForViewerDto>(company));
    }

    [HttpGet("companies"), ValidationFilter]
    public async Task<IActionResult> GetCompanies([Required, FromQuery] GetDeliveryCompanyQuery query)
    {
        var companies =
            string.IsNullOrEmpty(query.CompanyName)
                ? repository.GetAllCompanies()
                : repository.SearchCompaniesByName(query.CompanyName);

        return Ok(companies);
    }

    [HttpPost, ValidationFilter]
    public async Task<IActionResult> Create([Required, FromForm] DeliveryCompanyCreateQuery query)
    {
        var phoneNumber = PhoneNumberValueObject.Create(query.PhoneNumber);

        if (!Uri.TryCreate(query.WebSite, new UriCreationOptions(), out Uri? webSite)
            || phoneNumber is null)
            return BadRequest("Not a valid web site and/or phone number");

        var foundCompany = await repository.GetByAnyParam(query.Name, webSite, phoneNumber);

        if (foundCompany is not null)
            return BadRequest("A company with that name, number, or website already exists");

        var newCompany = DeliveryCompanyEntity.Create(
            name: query.Name,
            description: query.Description,
            webSite: webSite,
            phoneNum: phoneNumber);

        if (newCompany is null)
            return BadRequest(); //Вобще такой ситуации не будет, тк есть DataAn. атрибуты на query
        //+ еще проверяю номер и сайт на валидность в начале action, но похуй, пусть будет что ли

        await repository.Add(newCompany);
        return Ok();
    }
}