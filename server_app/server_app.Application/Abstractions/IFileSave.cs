using server_app.Domain.Model;

namespace server_app.Application.Abstractions;

public interface IFileSave
{
    Task<bool> Save(SavedFile file);
}