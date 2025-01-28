using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server_app.Domain.Entities.ProductCategories.Images;
using server_app.Domain.Model.Options;

namespace server_app.Application.MongoClient;

public class MongoDbClient : IMongoDbClient
{
    public MongoDbClient(IOptions<MongoDbOptions> options)
    {
        var optionsValue = options.Value;
        
        var client = new MongoDB.Driver.MongoClient(optionsValue.ConnectionString);
        var database = client.GetDatabase(optionsValue.DatabaseName);
        
        _image = database.GetCollection<ImageMongoEntity>(optionsValue.ImagesCollectionName);
    }
    private readonly  IMongoCollection<ImageMongoEntity> _image;

    public IMongoCollection<ImageMongoEntity> GetImagesCollection() => _image;
}