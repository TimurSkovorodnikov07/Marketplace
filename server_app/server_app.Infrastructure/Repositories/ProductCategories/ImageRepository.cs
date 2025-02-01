using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server_app.Application.Options;
using server_app.Application.Repositories;
using server_app.Domain.Entities.ProductCategories.Images;
using server_app.Domain.Model;

namespace server_app.Infrastructure.Repositories.ProductCategories;

public class ImageRepository : IImageRepository
{
    public ImageRepository(ILogger<ImageRepository> logger, IMongoDb db,
        IOptions<MongoDbOptions> options)
    {
        
        var optionsValue = options.Value;

        var client = new MongoDB.Driver.MongoClient(optionsValue.ConnectionString);
        var Database = client.GetDatabase(optionsValue.DatabaseName);
        
        
        _imageCollection = Database
            .GetCollection<ImageEntity>(options.Value.ImagesCollectionName);
    }

    private readonly IMongoCollection<ImageEntity> _imageCollection;

    public async Task<IEnumerable<ImageEntity>> GetByProductCategoryId(Guid porductCategoryId)
    {
        var foundImages = await _imageCollection.FindAsync(x => x.ProductCategoryId == porductCategoryId);
        return await foundImages.ToListAsync();
    }

    public async Task<ImageEntity?> GetById(Guid id)
    {
        var foundImages = await _imageCollection.FindAsync(x => x.Id == id);
        return await foundImages.FirstOrDefaultAsync();
    }

    public async Task<bool> Create(SavedFile file, Guid productId)
    {
        var newImage = await CreateImageEntity(file, productId);
        if (newImage == null) return false;
            
        await _imageCollection.InsertOneAsync(newImage);
        return true;
    }

    public async Task<bool> Create(List<SavedFile> files, Guid productId)
    {
        var newImages = new List<ImageEntity>();

        foreach (var file in files)
        {
            var newImage = await CreateImageEntity(file, productId);
            if (newImage == null) return false;
            
            newImages.Add(newImage);
        }

        if (newImages.Count > 0) return false;

        await _imageCollection.InsertManyAsync(newImages);
        return true;
    }

    private async Task<ImageEntity?> CreateImageEntity(SavedFile file, Guid productId)
    {
        using var memoryStream = new MemoryStream();
        await file.FileStream.CopyToAsync(memoryStream);

        var newImage = ImageEntity.Create(productId, file.MimeType, memoryStream.ToArray());
        return newImage;
    }

    public async Task<bool> DeleteById(Guid deletedId)
    {
        var deleteResult = await _imageCollection.DeleteOneAsync(x => x.Id == deletedId);
        return deleteResult.DeletedCount > 0;
    }

    public async Task<bool> DeleteAllByProductCategoryId(Guid categoryId)
    {
        var deleteResult = await _imageCollection.DeleteOneAsync(x => x.ProductCategoryId == categoryId);
        return deleteResult.DeletedCount > 0;
    }
}