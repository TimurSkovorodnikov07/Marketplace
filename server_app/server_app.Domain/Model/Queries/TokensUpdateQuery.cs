using System.ComponentModel.DataAnnotations;

namespace server_app.Domain.Model.Queries;

public class TokensUpdateQuery
{
    [Required] public string OldRefreshToken { get; set; }
    [Required] public Guid UserId { get; set; }
}