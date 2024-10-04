public class DeliveryCompanyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Uri WebSite { get; set; }
    public PhoneNumberValueObject PhoneNumber { get; set; }
}