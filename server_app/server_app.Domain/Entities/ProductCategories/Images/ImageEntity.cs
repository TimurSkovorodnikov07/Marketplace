using server_app.Domain.Validations;

namespace server_app.Domain.Entities.ProductCategories.Images;

public class ImageEntity : Entity
{
    public Guid ProductCategoryId { get; set; }
    public ProductCategoryEntity ProductCategory { get; set; }
    public string FileName { get; set; }
    public string MimeType { get; set; }

    public static ImageEntity? Create(Guid id, string fileName, Guid productId, string mimeType)
    {
        var newImage = new ImageEntity
        {
            Id = id,
            ProductCategoryId = productId,
            FileName = fileName,
            MimeType = mimeType
        };
        
        return ImageValidator.IsValid(newImage) ? newImage : null;
    }
}
