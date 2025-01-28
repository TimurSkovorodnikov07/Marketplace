using System.ComponentModel.DataAnnotations;
using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Entities.Users.Customer;

namespace server_app.Domain.Model.Dtos;

public class PurchasedProductCreateDto
{
    [Required] public ProductCategoryEntity Category { get; set; }
    [Required] public CustomerEntity Buyer { get; set; }
}