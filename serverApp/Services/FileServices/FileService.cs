public class FileService : IFileSave
{
    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    private readonly ILogger<FileService> _logger;

    public async Task Save(SavedFile file)
    {
        await using var stream = new FileStream(file.FullPath, FileMode.Create);
        await file.File.CopyToAsync(stream);
    }

    public async Task Save(IEnumerable<SavedFile> files)
    {
        foreach (var savedFile in files)
            await Save(savedFile);
    }

    public void Remove(string pathToFile)
    {
        File.Delete(pathToFile);
    }
}