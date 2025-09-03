using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Validations;

namespace server_app.Domain.Entities.ProductCategories.PurchasedProducts;

public class PurchasedProductEntity : Entity
{
    public ProductCategoryEntity Category { get; set; }
    public Guid CategoryId { get; set; }
    public CustomerEntity? Buyer { get; set; }
    public Guid? BuyerId { get; set; }
    public int PurchasedQuantity { get; set; } = 1;
    public decimal TotalSum { get; set; }
    public Guid MainImageId { get; set; }

    public DateTime PurchasedDate { get; set; }
    public DateTime MustDeliveredBefore { get; set; }
    public DateTime? DeliveredDate { get; set; }

    public static PurchasedProductEntity? Create(ProductCategoryEntity category, CustomerEntity buyer,
        DateTime mustDeliveredBefore, int purchasedQuantity, decimal totalSum, Guid mainImageId)
    {
        var newProduct = new PurchasedProductEntity
        {
            Category = category,
            Buyer = buyer,
            PurchasedDate = DateTime.UtcNow,
            MustDeliveredBefore = mustDeliveredBefore,
            DeliveredDate = null,
            PurchasedQuantity = purchasedQuantity,
            TotalSum = totalSum,
            MainImageId = mainImageId
        };

        return ProductValidator.IsValid(newProduct) ? newProduct : null;
    }
}