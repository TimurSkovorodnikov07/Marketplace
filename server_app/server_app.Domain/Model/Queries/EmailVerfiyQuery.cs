using System.ComponentModel.DataAnnotations;

namespace server_app.Domain.Model.Queries;

public class EmailVerifyQuery
{
    [Required] public Guid UserId { get; set; }
    [Required] public string Code { get; set; }
}