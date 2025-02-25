using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Validations;

namespace server_app.Domain.Entities.ProductCategories.DeliveryCompanies;

public class DeliveryCompanyEntity : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public WebSiteValueObject WebSite { get; set; }
    public PhoneNumberValueObject PhoneNumber { get; set; }

    public static DeliveryCompanyEntity? Create(string name, string description, WebSiteValueObject webSite,
        PhoneNumberValueObject phoneNum)
    {
        var newDeliveryCompany = new DeliveryCompanyEntity
        {
            Name = name,
            Description = description,
            WebSite = webSite,
            PhoneNumber = phoneNum
        };

        return DeliveryCompanyValidator.IsValid(newDeliveryCompany) ? newDeliveryCompany : null;
    }
}