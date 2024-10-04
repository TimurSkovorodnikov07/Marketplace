using Microsoft.EntityFrameworkCore;

public class ImagesService(
    FileService imageSer,
    MainDbContext context,
    IConfiguration conf)
    : IImageService
{
    private readonly string pathToFiles = conf["UserSecrets:PathToFiles"];

    public async Task<ImageEntity?> Get(Guid guid) => await context.Images
        .Include(i => i.ProductCategory)
        .FirstOrDefaultAsync(x => x.Id == guid);

    public async Task<ImageEntity?> GetByOwnerId(Guid productId) =>
        await context.Images
            .Include(i => i.ProductCategory)
            .FirstOrDefaultAsync(x => x.ProductId == productId);

    public IEnumerable<ImageEntity> GetImagesByOwnerId(Guid productId) =>
        context.Images
            .Include(i => i.ProductCategory)
            .Where(x => x.ProductId == productId);


    public async Task<bool> Save(IFormFile file, Guid productId) => await Save(file, productId, true);
    public async Task<bool> Save(IFormFile[] files, Guid productId) => await Save(files.ToList(), productId, true);

    public async Task<bool> Save(List<IFormFile> files, Guid productId, bool saveChange)
    {
        foreach (var file in files)
        {
            var res = await Save(file, productId);

            if (!res)
                return false;
        }

        if (saveChange)
            await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Save(IFormFile file, Guid productId, bool saveChange)
    {
        var newImage = ImageEntity.Create(pathToFiles, productId);

        if (newImage == null || ImageExtensionVerify(file) == false)
            return false;

        await context.Images.AddAsync(newImage);

        if (saveChange)
            await context.SaveChangesAsync();

        await imageSer.Save(file);
        return true;
    }

    private bool ImageExtensionVerify(IFormFile file)
    {
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };

        foreach (var type in allowedTypes)
            if (file.ContentType == type)
                return true;

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

        imageSer.Remove(image.Path);
        return true;
    }
}