public class DeliveryCompanyUpdatedDto
{
    public Guid Id { get; set; }
    public string NewName { get; set; }
    public string NewDescription { get; set; }
    public Uri NewWebSite { get; set; }
    public PhoneNumberValueObject NewPhoneNumber { get; set; }
}