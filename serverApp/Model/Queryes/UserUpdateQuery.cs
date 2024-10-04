using System.ComponentModel.DataAnnotations;

public class UserUpdateQuery
{
    [Required, StringLength(24)]
    public string NewName { get; set; }

    [Required, StringLength(45)]
    public string NewEmail{ get; set;}

    [Required, RegularExpression(UserLoginQuery.PASSWORDREGEX)]
    public string NewPassword { get; set;}
}
