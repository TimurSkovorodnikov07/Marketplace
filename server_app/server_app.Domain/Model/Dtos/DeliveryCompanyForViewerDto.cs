namespace server_app.Domain.Model.Dtos;

public class DeliveryCompanyForViewerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string WebSite { get; set; }
    public string PhoneNumber { get; set; }
}