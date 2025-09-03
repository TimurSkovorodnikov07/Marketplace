using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.ModelQueries;

public class ProductCategoryUpdateQuery
{
    [Required]
    public Guid Id { get; set; }
    
    [Required, StringLength(24)]
    public string NewName { get; set; }

    [StringLength(500)]
    public string? NewDescription { get; set; }
    
    [Required]
    public List<string> NewTags { get; set; }
    
    [Required, Range(0, int.MaxValue)]
    public decimal NewPrice { get; set; }
    
    [Required, Range(0, int.MaxValue)]
    public int NewQuantity { get; set; }
    
    [Required]
    public Guid NewDeliveryCompanyId { get; set; } 
}