using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductCategoryEntityConfigurations : IEntityTypeConfiguration<ProductCategoryEntity>
{
    public void Configure(EntityTypeBuilder<ProductCategoryEntity> builder)
    {
        builder.ToTable("product_categories");
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
        
        builder.HasOne(x => x.Owner).WithMany(x => x.ProductsCategories)
            .IsRequired().HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade).HasConstraintName("seller_constraint");
        
        builder.HasOne(x => x.DeliveryCompany).WithMany().HasForeignKey(x => x.DeliveryCompanyId)
            .OnDelete(DeleteBehavior.SetNull).HasConstraintName("delivery_company_constraint");
        
        builder.HasMany(x => x.Products).WithOne(x => x.Category)
            .OnDelete(DeleteBehavior.SetNull).HasConstraintName("delivery_company_constraint");
        
        //builder.HasMany(x => x.Images).WithOne().OnDelete(DeleteBehavior.SetNull).HasConstraintName("images_constraint");
        //Не нужный код, написал не думая и тут отношения, потому efcore создал еще и column ProductEntityId1 в images
        
        builder.HasIndex(x => x.Name);
    }
}