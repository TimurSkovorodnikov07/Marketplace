namespace server_app.Domain.Users.Tokens;

public class RefreshTokenEntity
{
    private RefreshTokenEntity(Guid userId, string tokenHash)
    {
        UserId = userId;
        TokenHash = tokenHash;
    }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; }

    public static RefreshTokenEntity Create(Guid userId, string tokenHash) => new (userId, tokenHash);
}