#nullable disable
using System.ComponentModel.DataAnnotations;

public class ProductCategoryCreateQuery
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    
    [Required]
    public TagsValueObject Tags { get; set; }

    [Required, Range(0, int.MaxValue)] 
    public decimal Price { get; set; }

    [Required, Range(1, int.MaxValue)] 
    public int Quantity { get; set; }

    [Required]
    public Guid DeliveryCompanyId { get; set; }
    
    [Required]
    public List<IFormFile> Images{ get; set; }
}