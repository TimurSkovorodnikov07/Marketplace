public class FileService : IFileSave
{
    public FileService(IConfiguration conf, ILogger<FileService> logger)
    {
        _logger = logger;
        _pathToFiles = conf["UserSecrets:PathToFiles"];

        if (string.IsNullOrEmpty(_pathToFiles))
            throw new NullReferenceException();
    }
    private readonly ILogger<FileService> _logger;
    private readonly string _pathToFiles;
    
    public virtual async Task<bool> Save(IFormFile file)
    {
        await using var stream = new FileStream(_pathToFiles, FileMode.Create);
        await file.CopyToAsync(stream);
        
        return true;
    }
    
    public void Remove(string pathToFile)
    {
        File.Delete(pathToFile);
    }
}