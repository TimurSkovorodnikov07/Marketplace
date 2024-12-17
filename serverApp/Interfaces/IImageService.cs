public interface IImageService
{
    Task<bool> Save(Guid productId, IFormFile[] files);
}