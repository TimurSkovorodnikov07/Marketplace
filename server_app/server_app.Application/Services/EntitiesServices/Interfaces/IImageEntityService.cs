using Microsoft.AspNetCore.Http;
using server_app.Domain.Entities.ProductCategories.Images;

namespace server_app.Application.Services.EntitiesServices.Interfaces;
public interface IImageEntityService
{
    Task<bool> Save(Guid productId, IFormFile[] files);
    Task<bool> Save(Guid productId, IEnumerable<IFormFile> files);
    Task<ImageEntity?> Get(Guid guid);
    Task<ImageEntity?> GetByOwnerId(Guid productId);
    IEnumerable<ImageEntity> GetImagesByOwnerId(Guid productId);
    Task<bool> Remove(Guid guid);
    Task<bool> RemoveByCategoryId(Guid categoryId);
}