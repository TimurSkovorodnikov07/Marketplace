using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PurchasedProductEntityConfigurations : IEntityTypeConfiguration<PurchasedProductEntity>
{
    public void Configure(EntityTypeBuilder<PurchasedProductEntity> builder)
    {
        builder.ToTable("purchased_products");
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.PurchasedDate).IsRequired().HasColumnName("purchased_date");
        builder.Property(x => x.MustDeliveredBefore).IsRequired().HasColumnName("must_delivered_before");
        builder.Property(x => x.Delivered).IsRequired().HasColumnName("delivered");

        builder.Property(x => x.CategoryId).HasColumnName("category_id");//Отношения уже написаны в ProductCategoryEntityConfigurations
        builder.Property(x => x.BuyerId).HasColumnName("buyer_id");
        builder.HasOne(x => x.Buyer).WithMany(x => x.Purchases).HasForeignKey(x => x.BuyerId)
            .OnDelete(DeleteBehavior.SetNull).HasConstraintName("buyer_constraint");
    }
}