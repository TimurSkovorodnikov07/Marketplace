using System.ComponentModel.DataAnnotations;

public class GetDeliveryCompanyQuery
{
    [StringLength(20)] public string? CompanyName { get; set; }    
}