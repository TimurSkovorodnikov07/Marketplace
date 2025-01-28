using MongoDB.Driver;
using server_app.Domain.Entities.ProductCategories.Images;

namespace server_app.Application.MongoClient;

public interface IMongoDbClient
{
    public IMongoCollection<ImageMongoEntity> GetImagesCollection();
}