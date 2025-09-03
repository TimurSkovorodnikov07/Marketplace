using System.ComponentModel.DataAnnotations;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Entities.Users.Seller;

namespace server_app.Domain.Model.Dtos;

public class ProductCategoryCreateDto
{
    [Required, StringLength(24)] public string Name { get; set; }
    [StringLength(500)] public string? Description { get; set; }

    [Required] public TagsValueObject Tags { get; set; }

    [Required, Range(0, int.MaxValue)] public decimal Price { get; set; }
    [Required, Range(0, int.MaxValue)] public int Quantity { get; set; }

    
    [Required] public DeliveryCompanyEntity DeliveryCompany { get; set; }
    [Required] public SellerEntity Owner { get; set; }
    [Required] public List<SavedFile> Images { get; set; }
}