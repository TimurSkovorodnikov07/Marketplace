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
        _logger = logger;

        var client = new MongoDB.Driver.MongoClient(optionsValue.ConnectionString);
        var Database = client.GetDatabase(optionsValue.DatabaseName);


        _imageCollection = Database
            .GetCollection<ImageEntity>(options.Value.ImagesCollectionName);
    }

    private readonly ILogger<ImageRepository> _logger;
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

    public async Task<(bool, Guid?)> Save(SavedFile file, Guid productId)
    {
        var newImage = await CreateImageEntity(file, productId);
        if (newImage == null) return (false, null);

        await _imageCollection.InsertOneAsync(newImage);
        return (true, newImage.Id);
    }

    public async Task<(bool, Guid?)> Save(List<SavedFile> files, Guid productId)
    {
        var newImages = new List<ImageEntity>();
        Guid? mainImageId = null;

        foreach (var file in files)
        {
            var newImage = await CreateImageEntity(file, productId);
            if (newImage == null)
            {
                _logger.LogError("Couldn't save The Images. One image in the list is null");
                return (false, null);
            }

            mainImageId ??= newImage.Id;
            newImages.Add(newImage);
        }

        if (newImages.Count <= 0)
        {
            _logger.LogError("Couldn't save The Images. The Images Count is 0");
            return (false, null);
        }

        await _imageCollection.InsertManyAsync(newImages);
        return (true, mainImageId);
    }

    private async Task<ImageEntity?> CreateImageEntity(SavedFile file, Guid productId)
    {
        _logger.LogTrace($"Create NEW Images(product id: {productId}):");
        try
        {
            var couldNotCreateText = "Couldn't create the image";
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await file.FileStream.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }
            _logger.LogTrace("Image:");
            _logger.LogTrace($"Mime type: {file.MimeType}");
            _logger.LogTrace($"FileData Length: {fileData.Length}");

            if (fileData.Length <= 0)
            {
                _logger.LogError($"File data is empty. {couldNotCreateText}");
                return null;
            }
            
            var newImage = ImageEntity.Create(productId, file.MimeType, fileData);

            if (newImage == null)
                _logger.LogCritical(couldNotCreateText);
            else
                _logger.LogTrace($"The new image has been created, his id: {newImage.Id}");
            
            return newImage;
        }
        catch (Exception ex)
        {
            _logger.LogError("Couldn't save The Image");
            _logger.LogError(ex.Message);
            throw;
        }
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