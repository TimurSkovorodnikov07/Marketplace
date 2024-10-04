using System.ComponentModel.DataAnnotations;

public class ProductCategoryUpdateQuery
{
    [Required]
    public Guid Id { get; set; }
    
    [Required, StringLength(24)]
    public string NewName { get; set; }
    
    [Required, StringLength(500)]
    public string NewDescription { get; set; }
    
    [Required]
    public TagsValueObject NewTags { get; set; }
    
    [Required, Range(0, int.MaxValue)]
    public decimal NewPrice { get; set; }
    
    [Required, Range(0, int.MaxValue)]
    public int NewQuantity { get; set; }
    
    [Required]
    public Guid NewDeliveryCompanyId { get; set; } 
}