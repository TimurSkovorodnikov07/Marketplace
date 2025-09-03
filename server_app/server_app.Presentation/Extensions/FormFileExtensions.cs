using server_app.Domain.Model;

namespace server_app.Presentation.Extensions;

public static class FormFileExtensions
{
    public static List<SavedFile> ToSavedFile(this List<IFormFile> formFiles)
    {
        var savedFiles = new List<SavedFile>();

        if (formFiles == null || formFiles.Count == 0)
            return savedFiles;

        foreach (var file in formFiles)
        {
            var memoryStream = new MemoryStream(); 
            file.CopyTo(memoryStream);

            // Reset position before reading
            memoryStream.Position = 0;

            var newSavedFile = new SavedFile(file.FileName, memoryStream, file.ContentType);
            savedFiles.Add(newSavedFile);
        }

        return savedFiles;
    }
}
