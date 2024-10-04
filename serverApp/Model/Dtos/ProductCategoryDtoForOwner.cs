public class ProductCategoryDtoForOwner : ProductCategoryDto
{
    public Guid Id { get; set;}
    public Guid DeliveryCompanyId { get; set; }
    public List<Guid> ImagesIdentifiers { get; set; }
}