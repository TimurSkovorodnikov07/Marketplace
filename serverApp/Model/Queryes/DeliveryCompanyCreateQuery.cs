using System.ComponentModel.DataAnnotations;

public class DeliveryCompanyCreateQuery
{
    [Required, MaxLength(20)] public string Name { get; set; }
    [Required, MaxLength(500)] public string Description { get; set; }
    [Required] public string WebSite { get; set; }
    [Required] public string PhoneNumber { get; set; }
}