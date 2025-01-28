namespace server_app.Domain.Model.Dtos;

public class UserUpdateDto : IUserUpdate
{
    public Guid Id { get; set; }
    public string NewName { get; set; }
    public string NewEmail { get; set; }
    public string NewPassword { get; set; }
}