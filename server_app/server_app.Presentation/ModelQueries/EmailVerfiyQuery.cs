using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.ModelQueries;

public class EmailVerifyQuery
{
    [Required] public Guid UserId { get; set; }
    [Required] public string Code { get; set; }
}