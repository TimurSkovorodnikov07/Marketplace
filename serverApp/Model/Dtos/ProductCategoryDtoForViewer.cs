#nullable disable
public class ProductCategoryDtoForViewer : ProductCategoryDto
{
    public Guid DeliveryCompanyId { get; set; }
    public List<Guid> ImagesId { get; set; }
    
    public SelleDtorForViewer SelleDtorForViewer { get; set; }
    public List<Guid> ImagesIdentifiers { get; set; }
}
