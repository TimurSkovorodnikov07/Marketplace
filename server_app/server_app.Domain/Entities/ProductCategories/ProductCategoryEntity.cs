using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.PurchasedProducts;
using server_app.Domain.Entities.ProductCategories.Reviews;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Entities.Users.Seller;
using server_app.Domain.Model.Dtos;
using server_app.Domain.Validations;

namespace server_app.Domain.Entities.ProductCategories;

public class ProductCategoryEntity : Entity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public TagsValueObject Tags { get; set; }

    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public List<PurchasedProductEntity> PurchasedProducts { get; set; }
    public DeliveryCompanyEntity? DeliveryCompany { get; set; }
    public Guid DeliveryCompanyId { get; set; }

    public SellerEntity Owner { get; set; }
    public Guid OwnerId { get; set; }
    public List<ReviewEntity> Reviews { get; set; }
    public int TotalEstimation { get; set; }
    public int EstimationCount { get; set; }

    private Guid _mainImageId;
    public Guid MainImageId
    {
        get => _mainImageId;
        set
        {
            if (_mainImageId != Guid.Empty)
                throw new InvalidOperationException("The value is already set and cannot be changed.");
                
            _mainImageId = value;
        }
    }



    public static ProductCategoryEntity? Create(ProductCategoryCreateDto dto)
    {
        var newCategory = new ProductCategoryEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            Tags = dto.Tags,
            Price = dto.Price,
            Quantity = dto.Quantity,
            DeliveryCompany = dto.DeliveryCompany,
            Owner = dto.Owner,
            EstimationCount = 0,
            TotalEstimation = 0,
        };
        return ProductCategoryValidator.IsValid(newCategory)
            ? newCategory
            : null;
    }
}