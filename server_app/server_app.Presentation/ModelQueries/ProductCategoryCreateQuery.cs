#nullable disable
using System.ComponentModel.DataAnnotations;
using server_app.Presentation.Filters.ValidatorAttributes;

namespace server_app.Presentation.ModelQueries;

public class ProductCategoryCreateQuery
{
    [Required, StringLength(24)] public string Name { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }

    [Required] public List<string> Tags { get; set; }

    [Required, Range(1, int.MaxValue)] public decimal Price { get; set; }

    [Required, Range(1, int.MaxValue)] public int Quantity { get; set; }

    [Required] public Guid DeliveryCompanyId { get; set; }

    [Required, FormFileListIsNotEmptyValidatorFilter] public List<IFormFile> Images { get; set; }
}