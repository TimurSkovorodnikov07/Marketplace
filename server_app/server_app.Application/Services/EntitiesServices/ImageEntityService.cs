using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using server_app.Application.Services.EntitiesServices.Interfaces;
using server_app.Application.Services.FileServices;
using server_app.Domain;
using server_app.Domain.Entities.ProductCategories.Images;
using server_app.Domain.Model;
using server_app.Domain.Validations;

namespace server_app.Application.Services.EntitiesServices;

public class ImageEntityService(
    ImageMongoDbService imageMongoDbService,
    MainDbContext context,
    ILogger<ImageEntityService> logger)
    : IImageEntityService
{
    public async Task<ImageEntity?> Get(Guid guid) =>
        await context.Images
            .FirstOrDefaultAsync(x => x.Id == guid);

    public async Task<ImageEntity?> GetByOwnerId(Guid productId) =>
        await context.Images
            .FirstOrDefaultAsync(x => x.ProductCategoryId == productId);

    public IEnumerable<ImageEntity> GetImagesByOwnerId(Guid productId) =>
        context.Images
            .Where(x => x.ProductCategoryId == productId);


    public async Task<bool> Save(Guid productId, IEnumerable<IFormFile> files) =>
        await Save(productId, files.ToArray());

    public async Task<bool> Save(Guid productId, params IFormFile[] files)
    {
        var savedFiles = new List<SavedFile>();
        var savingResult = true;

        foreach (var file in files)
        {
            var id = Guid.NewGuid();
            var fileName = $"{id}.{Path.GetFileName(file.FileName)}";
            var savedFile = new SavedFile(file);

            if (TryCorrectImageMimeType(file, out string? mimeType) == false)
                return false;

            var newImage = ImageEntity.Create(id, fileName, productId, mimeType);

            if (newImage == null)
                return false;

            await context.Images.AddAsync(newImage);
            savedFiles.Add(savedFile);

            var res = await imageMongoDbService.Save(id, mimeType, savedFile);
            if (res == false) savingResult = false;
        }

        if (savingResult)
            await context.SaveChangesAsync();

        logger.LogTrace(@"Images of the product category with Id=""{x}"" were saved.", productId);
        return savingResult;
    }

    private bool TryCorrectImageMimeType(IFormFile file, out string? mimeType)
    {
        if (ImageValidator.IsMimeTypeAllowed(file.ContentType))
        {
            mimeType = file.ContentType;
            return true;
        }

        mimeType = null;
        return false;
    }


    public async Task<bool> Remove(Guid guid)
    {
        var image = await context.Images
            .FirstOrDefaultAsync(x => x.Id == guid);

        if (image == null)
            return false;

        var isDeletedFromMongo = await imageMongoDbService.Remove(image);

        if (isDeletedFromMongo)
        {
            context.Images.Remove(image);
            await context.SaveChangesAsync();
        }

        return isDeletedFromMongo;
    }

    public async Task<bool> RemoveByCategoryId(Guid categoryId)
    {
        var images = await context.Images
            .Where(x => x.ProductCategoryId == categoryId).ToListAsync();

        if (images.Count <= 0)
            return false;

        foreach (var i in images)
        {
            context.Images.Remove(i);
            await imageMongoDbService.Remove(i);
        }

        await context.SaveChangesAsync();
        return true;
    }
}