public class PurchasedProductEntity : Entity
{
    public ProductCategoryEntity Category { get; set; }
    public Guid CategoryId { get; set; }
    public CustomerEntity? Buyer { get; set; }
    public Guid? BuyerId { get; set; }


    public DateTime PurchasedDate { get; set; }
    public DateTime MustDeliveredBefore { get; set; }
    public DateTime Delivered { get; set; }

    public static PurchasedProductEntity? Create(ProductCategoryEntity category, CustomerEntity buyer,
        DateTime mustDeliveredBefore)
    {
        var newProduct = new PurchasedProductEntity
        {
            Category = category,
            Buyer = buyer,
            PurchasedDate = DateTime.UtcNow,
            MustDeliveredBefore = mustDeliveredBefore,
        };

        return ProductValidator.IsValid(newProduct) ? newProduct : null;
    }
}