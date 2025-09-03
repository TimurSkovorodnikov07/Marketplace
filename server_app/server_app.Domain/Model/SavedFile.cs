namespace server_app.Domain.Model;

public class SavedFile(string fileName, Stream fileStream, string mimeType)
{
    public string FileName { get; set; } = fileName;
    public Stream FileStream { get; set; } = fileStream;
    public string MimeType { get; set; } = mimeType;
}