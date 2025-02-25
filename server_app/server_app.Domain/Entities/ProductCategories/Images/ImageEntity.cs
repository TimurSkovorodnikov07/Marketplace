using server_app.Domain.Validations;

namespace server_app.Domain.Entities.ProductCategories.Images;

public class ImageEntity
{
    //ImageEntity didn't inheritance from Entity because it need for the normal working of BsonClassMap
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ProductCategoryId { get; set; }

    public string MimeType { get; set; }

    public byte[] ImageData { get; set; }

    public static ImageEntity? Create(Guid productCategoryId, string mimeType, byte[] imageBytes)
    {
        var newImage = new ImageEntity()
        {
            Id = Guid.NewGuid(),
            ProductCategoryId = productCategoryId,
            MimeType = mimeType,
            ImageData = imageBytes
        };
        return ImageValidator.IsValid(newImage) ? newImage : null;
    }
}