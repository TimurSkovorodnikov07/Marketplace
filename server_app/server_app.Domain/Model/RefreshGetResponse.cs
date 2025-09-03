namespace server_app.Domain.Model;

public class RefreshGetResponse
{
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
}