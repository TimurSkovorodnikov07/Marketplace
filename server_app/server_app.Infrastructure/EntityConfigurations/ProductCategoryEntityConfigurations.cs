using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities.ProductCategories;

namespace server_app.Infrastructure.EntityConfigurations;

public class ProductCategoryEntityConfigurations : IEntityTypeConfiguration<ProductCategoryEntity>
{
    public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(24).HasColumnName("name");
        builder.Property(x => x.Description).HasMaxLength(500).HasColumnName("description");
        builder.Property(x => x.Price).IsRequired().HasColumnName("price");
        builder.Property(x => x.Quantity).IsRequired().HasColumnName("quantity");

        builder.OwnsOne(
            x => x.Tags,
            properties =>
            {
                properties
                    .Property(x => x.Tags)
                    .IsRequired()
                    .HasColumnType("varchar[]")
                    .HasColumnName("tags");
            }
        );

        builder.Property(x => x.OwnerId).IsRequired().HasColumnName("seller_id");
        builder
            .Property(x => x.DeliveryCompanyId)
            .IsRequired()
            .HasColumnName("delivery_company_id");

        builder
            .Property(x => x.TotalEstimation)
            .IsRequired()
            .HasDefaultValue(0)
            .HasColumnName("total_estimation");
        builder
            .Property(x => x.EstimationCount)
            .IsRequired()
            .HasDefaultValue(0)
            .HasColumnName("estimation_count");
        builder.Property(x => x.MainImageId).IsRequired().HasColumnName("main_image_id");

        builder
            .HasOne(x => x.Owner)
            .WithMany(x => x.ProductsCategories)
            .IsRequired()
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("seller_constraint");

        builder
            .HasOne(x => x.DeliveryCompany)
            .WithMany()
            .HasForeignKey(x => x.DeliveryCompanyId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("delivery_company_constraint");

        builder
            .HasMany(x => x.PurchasedProducts)
            .WithOne(x => x.Category)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("purchased_products_constraint");

        builder.HasIndex(x => x.Name);
        builder.ToTable(
            "product_categories",
            tableBuilder =>
            {
                tableBuilder.HasCheckConstraint(
                    "CK_ProductCategories_TotalEstimation",
                    "total_estimation >= 0 AND total_estimation <= 10"
                );

                tableBuilder.HasCheckConstraint(
                    "CK_ProductCategories_EstimationCount",
                    "estimation_count >= 0"
                );
            }
        );
    }
}
