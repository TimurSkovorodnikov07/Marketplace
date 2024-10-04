using System.ComponentModel.DataAnnotations;

public class ProductBuyQuery
{
    [Required] public Guid ProductCategoryId { get; set; }
}