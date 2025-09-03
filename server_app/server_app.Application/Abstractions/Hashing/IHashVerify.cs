namespace server_app.Application.Abstractions.Hashing;

public interface IHasher
{
    public string? Hashing(string str);
}