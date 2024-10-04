public class ImageEntity : Entity
{
    public Guid ProductId { get; set; }
    public ProductCategoryEntity ProductCategory { get; set; }
    
    public string Path { get; set; }

    public static ImageEntity? Create(string fileSaveDirectoryPath, Guid productId)
    {
        var newImage = new ImageEntity();
        newImage.ProductId = productId;
        newImage.Path = $"{fileSaveDirectoryPath}/{newImage.Id}";
        
        return ImageValidator.IsValid(newImage) ? newImage : null;
    }
}
