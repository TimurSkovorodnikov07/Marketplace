using System.ComponentModel.DataAnnotations;

public class TokensUpdateQuery
{
    [Required] public string OldRefreshToken { get; set; }
    [Required] public Guid UserId { get; set; }
}