using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using server_app.Application.MongoClient;
using server_app.Domain.Entities.ProductCategories.Images;
using server_app.Domain.Model;

namespace server_app.Application.Services.FileServices;

public class ImageMongoDbService(
    ILogger<ImageMongoDbService> logger,
    IMongoDbClient mongoClient)
{
    private readonly IMongoCollection<ImageMongoEntity> _imagesCollection = mongoClient.GetImagesCollection();

    public async Task<ImageMongoEntity?> GetImage(Guid id)
    {
        var foundImage = await _imagesCollection.FindAsync(x => x.GuidId == id);
        return foundImage.FirstOrDefault();
    }

    public async Task<bool> Save(Guid id, string mimeType, SavedFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.File.CopyToAsync(memoryStream);
        
        var newImage = ImageMongoEntity.Create(id, mimeType, memoryStream.ToArray());
        if (newImage == null) return false;

        await _imagesCollection.InsertOneAsync(newImage);
        return true;
    }

    public async Task<bool> Remove(ImageEntity image)
    {
        var deleteResult = await _imagesCollection.DeleteOneAsync(x => x.GuidId == image.Id);
        return deleteResult.DeletedCount > 0;
    }
}