using System.ComponentModel.DataAnnotations;

public class SellerRegistrationQuery : UserRegistrationQuery
{
    [Required, StringLength(500)] public string Description { get; set; }
}