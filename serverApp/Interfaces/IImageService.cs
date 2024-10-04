public interface IImageService
{
    Task<bool> Save(IFormFile file, Guid productId);
}