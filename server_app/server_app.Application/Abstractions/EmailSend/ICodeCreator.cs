namespace server_app.Application.Abstractions.EmailSend;

public interface ICodeCreator
{
    public string? Create(int length);
}