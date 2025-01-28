#nullable disable
namespace server_app.Domain.Model.Dtos;

public class ProductCategoryDtoForViewer : ProductCategoryDto
{
    public Guid OwnerId { get; set; }
}
