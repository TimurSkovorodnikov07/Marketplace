using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Model.Dtos;

namespace server_app.Application.Services.EntitiesServices.Interfaces;

public interface IDeliveryCompanyService : IEntityService<DeliveryCompanyEntity, DeliveryCompanyUpdatedDto>
{
    Task<DeliveryCompanyEntity?> GetByAnyParam(string name, Uri webSite, PhoneNumberValueObject phoneNum);
    IEnumerable<DeliveryCompanyForViewerDto> SearchCompaniesByName(string str);
    IEnumerable<DeliveryCompanyForViewerDto> GetAllCompanies();
}