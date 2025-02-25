using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Model.Dtos;

namespace server_app.Application.Repositories;

public interface IDeliveryCompanyRepository : IEntityRepository<DeliveryCompanyEntity, DeliveryCompanyUpdatedDto>
{
    Task<DeliveryCompanyEntity?> GetByAnyParam(string name, string webSite, PhoneNumberValueObject phoneNum);
    IEnumerable<DeliveryCompanyForViewerDto> SearchCompaniesByName(string str);
    IEnumerable<DeliveryCompanyForViewerDto> GetAllCompanies();
}