using Microsoft.EntityFrameworkCore;
public class ImagesService(
    FileService fileService,
    MainDbContext context,
    IConfiguration conf,
    ILogger<ImagesService> logger)
    : IImageService
{
    private readonly string pathToFiles = conf["UserSecrets:PathToFiles"];

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

        foreach (var savedFile in files)
        {
            var newGuid = Guid.NewGuid();
            var fileName = $"{newGuid}.{Path.GetFileName(savedFile.FileName)}";
            var fullPath = Path.Combine(pathToFiles, fileName);

            if (!ImageExtensionVerify(savedFile, out string? mimeType))
                return false;

            var newImage = ImageEntity.Create(fileName, fullPath, productId, mimeType);

            if (newImage == null)
                return false;

            newImage.Id = newGuid;
            await context.Images.AddAsync(newImage);

            savedFiles.Add(new SavedFile
                {
                    FullPath = fullPath,
                    File = savedFile,
                }
            );
        }

        logger.LogTrace(@"images of the product category with Id=""{x}"" were saved.", productId);

        await context.SaveChangesAsync();
        await fileService.Save(savedFiles);
        return true;
    }
    
    private bool ImageExtensionVerify(IFormFile file, out string? mimeType)
    {
        foreach (var type in ImageValidator.imageAllowedTypes)
            if (file.ContentType == type)
            {
                mimeType = type;
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

        context.Images.Remove(image);
        await context.SaveChangesAsync();

        fileService.Remove(image.Path);
        return true;
    }
}