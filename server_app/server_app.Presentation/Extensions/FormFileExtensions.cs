using server_app.Domain.Model;

namespace server_app.Presentation.Extensions;

public static class FormFileExtensions
{
    public static List<SavedFile> ToSavedFile(this List<IFormFile> formFiles)
    {
        var savedFiles = new List<SavedFile>();

        if (formFiles == null || formFiles.Count <= 0)
            return savedFiles;
        
        foreach (var file in formFiles)
        {
            using var stream = file.OpenReadStream();
            var newSavedFile = new SavedFile(file.FileName, stream, file.ContentType);
            savedFiles.Add(newSavedFile);            
        }
        return savedFiles;
    }
}