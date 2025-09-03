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
        var webSite = WebSiteValueObject.Create(query.WebSite);

        if (phoneNumber is null) return BadRequest("The phone number isn't valid");
        if (webSite is null) return BadRequest("The website isn't valid");


        var foundCompany = await repository.GetByAnyParam(query.Name, webSite.WebSiteValue, phoneNumber);

        if (foundCompany is not null)
            return BadRequest("A company with that name, number, or website already exists");

        var newCompany = DeliveryCompanyEntity.Create(
            name: query.Name,
            description: query.Description,
            webSite: webSite,
            phoneNum: phoneNumber);

        if (newCompany is null)
            return BadRequest("Created Delivery Company isn't valid");

        await repository.Add(newCompany);
        return Ok();
    }

    [HttpDelete("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Remove([Required] Guid guid)
    {
        var result = await repository.Remove(guid);
        return result ? Ok() : BadRequest();
    }
}