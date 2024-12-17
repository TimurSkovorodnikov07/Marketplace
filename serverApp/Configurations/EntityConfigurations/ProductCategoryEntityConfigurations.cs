using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductCategoryEntityConfigurations : IEntityTypeConfiguration<ProductCategoryEntity>
{
    public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
    {
        builder.ToTable("product_categories", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_ProductCategories_TotalEstimation",
                "total_estimation >= 0 AND total_estimation <= 10");
            
            tableBuilder.HasCheckConstraint("CK_ProductCategories_EstimationCount",
                "estimation_count >= 0");

            //Хз в чем дело, код правильный вроде, чето писал при апдейте бд: "constraint "CK_ProductCategories_EstimationCount" of relation "product_categories" does not exist"
            //Снес нахуй все миграции и бд, ебал efcore
            //https://stackoverflow.com/questions/13232777/check-constraint-entity-framework
        });
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(24).HasColumnName("name");
        builder.Property(x => x.Description).HasMaxLength(500).HasColumnName("description");
        builder.Property(x => x.Price).IsRequired().HasColumnName("price");
        builder.Property(x => x.Quantity).IsRequired().HasColumnName("quantity");
        

        builder.OwnsOne(x => x.Tags, properties =>
        {
            properties.Property(x => x.Tags).IsRequired()
                .HasColumnType("varchar[]").HasColumnName("tags");
        });
        
        //Много картинок(hasMany => Images) могут имтеь лишь одину катег продуктов(WithOne)
        builder.Property(x => x.OwnerId).IsRequired().HasColumnName("seller_id");
        builder.Property(x => x.DeliveryCompanyId).IsRequired().HasColumnName("delivery_company_id");
        
        builder.Property(x => x.TotalEstimation).IsRequired()
            .HasDefaultValue(0).HasColumnName("total_estimation");
        builder.Property(x => x.EstimationCount).IsRequired()
            .HasDefaultValue(0).HasColumnName("estimation_count");
        
        builder.HasOne(x => x.Owner).WithMany(x => x.ProductsCategories)
            .IsRequired().HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade).HasConstraintName("seller_constraint");
        
        builder.HasOne(x => x.DeliveryCompany).WithMany().HasForeignKey(x => x.DeliveryCompanyId)
            .OnDelete(DeleteBehavior.SetNull).HasConstraintName("delivery_company_constraint");
        
        builder.HasMany(x => x.PurchasedProducts).WithOne(x => x.Category)
            .OnDelete(DeleteBehavior.SetNull).HasConstraintName("purchased_products_constraint");
         
        //builder.HasMany(x => x.Images).WithOne().OnDelete(DeleteBehavior.SetNull).HasConstraintName("images_constraint");
        //Не нужный код, написал не думая и тут отношения, потому efcore создал еще и column ProductEntityId1 в images
        
        builder.HasIndex(x => x.Name);
    }
}