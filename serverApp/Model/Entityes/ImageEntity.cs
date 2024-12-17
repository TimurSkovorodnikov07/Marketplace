public class ImageEntity : Entity
{
    public Guid ProductCategoryId { get; set; }
    public ProductCategoryEntity ProductCategory { get; set; }
    public string FileName { get; set; }
    public string Path { get; set; }
    public string MimeType { get; set; }

    public static ImageEntity? Create(string fileName, string path, Guid productId, string mimeType)
    {
        var newImage = new ImageEntity
        {
            ProductCategoryId = productId,
            FileName = fileName,
            Path = path,
            MimeType = mimeType
        };
        
        return ImageValidator.IsValid(newImage) ? newImage : null;
    }
}
