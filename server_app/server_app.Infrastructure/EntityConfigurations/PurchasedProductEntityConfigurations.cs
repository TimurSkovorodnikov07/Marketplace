using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities.ProductCategories.PurchasedProducts;

namespace server_app.Infrastructure.EntityConfigurations;

public class PurchasedProductEntityConfigurations : IEntityTypeConfiguration<PurchasedProductEntity>
{
    public void Configure(EntityTypeBuilder<PurchasedProductEntity> builder)
    {
        builder.ToTable("purchased_products");
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.PurchasedDate).IsRequired().HasColumnName("purchased_date");
        builder.Property(x => x.PurchasedQuantity).HasColumnName("purchased_quantity");
        builder.Property(x => x.TotalSum).HasColumnName("total_sum");
        builder.Property(x => x.MustDeliveredBefore).IsRequired().HasColumnName("must_delivered_before");
        builder.Property(x => x.DeliveredDate).HasColumnName("delivered_date");
        builder.Property(x => x.MainImageId).HasColumnName("main_image_id");

        builder.Property(x => x.CategoryId).HasColumnName("category_id");//Отношения уже написаны в ProductCategoryEntityConfigurations
        builder.Property(x => x.BuyerId).HasColumnName("buyer_id");
        builder.HasOne(x => x.Buyer).WithMany(x => x.Purchases).HasForeignKey(x => x.BuyerId)
            .OnDelete(DeleteBehavior.SetNull).HasConstraintName("buyer_constraint");
    }
}