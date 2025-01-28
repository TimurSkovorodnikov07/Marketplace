using System.ComponentModel.DataAnnotations;

namespace server_app.Domain.Model.Queries;

public class SellerRegistrationQuery : UserRegistrationQuery
{
    [Required, StringLength(500)] public string Description { get; set; }
}