public class HashingManagerService : IHasher, IHashVerify
{
    public string? Hashing(string str) => string.IsNullOrEmpty(str)
        ? null
        : BCrypt.Net.BCrypt.HashPassword(str);

    public bool Verify(string str, string hashStr) => BCrypt.Net.BCrypt.Verify(str, hashStr);
}