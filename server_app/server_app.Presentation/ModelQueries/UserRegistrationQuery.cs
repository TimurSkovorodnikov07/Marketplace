using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.ModelQueries;

public abstract class UserRegistrationQuery
{
    [Required, StringLength(24)]
    public string Name { get; set; }

    
    [Required, StringLength(45)] 
    public string Email { get; set; }

    
    [Required, RegularExpression(UserLoginQuery.PASSWORDREGEX)]
    public string Password { get; set; }
}