using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Entities.ProductCategories.ValueObjects;

namespace server_app.Domain.Model.Dtos;

public class ProductCategoryUpdateDto
{
    public Guid Id { get; set; }
    public string NewName { get; set; }
    public string? NewDescription { get; set; }
    public TagsValueObject NewTags { get; set; }
    public decimal NewPrice { get; set; }
    public int NewQuantity { get; set; }
    public Guid NewDeliveryCompanyId { get; set; }
}