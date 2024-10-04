public class DeliveryCompanyEntity : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Uri WebSite { get; set; }
    public PhoneNumberValueObject PhoneNumber { get; set; }

    public static DeliveryCompanyEntity? Create(string name, string description, Uri webSite,
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

    public static DeliveryCompanyEntity? Create(DeliveryCompanyCreateQuery createQuery, Uri webSite,
        PhoneNumberValueObject phoneNum) => Create(createQuery.Name, createQuery.Description, webSite, phoneNum);
}