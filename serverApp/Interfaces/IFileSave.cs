public interface IFileSave
{
    Task<bool> Save(IFormFile file);
}