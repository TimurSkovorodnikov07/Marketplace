using server_app.Domain.Entities;

public abstract class UserEntity : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public bool EmailVerify { get; set; }
    public string PasswordHash { get; set; }
}
