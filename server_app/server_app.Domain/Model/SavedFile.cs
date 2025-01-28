using Microsoft.AspNetCore.Http;

namespace server_app.Domain.Model;

public class SavedFile(IFormFile file)
{
    public IFormFile File { get; set; } = file;
}