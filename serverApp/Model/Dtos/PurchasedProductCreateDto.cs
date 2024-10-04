using System.ComponentModel.DataAnnotations;

public class PurchasedProductCreateDto
{
    [Required] public ProductCategoryEntity Category { get; set; }
    [Required] public CustomerEntity Buyer { get; set; }
}