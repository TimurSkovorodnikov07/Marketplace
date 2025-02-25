using server_app.Domain.Entities.ProductCategories.Images;
using server_app.Domain.Model;
using server_app.Domain.Model.Dtos;

namespace server_app.Application.Repositories;

public interface IImageRepository
{
    Task<IEnumerable<ImageEntity>> GetByProductCategoryId(Guid id);
    Task<ImageEntity?> GetById(Guid id);

    Task<(bool, Guid?)> Save(SavedFile file, Guid productId);
    Task<(bool, Guid?)> Save(List<SavedFile> files, Guid productId);
    Task<bool> DeleteById(Guid id);
    Task<bool> DeleteAllByProductCategoryId(Guid categoryId);
}