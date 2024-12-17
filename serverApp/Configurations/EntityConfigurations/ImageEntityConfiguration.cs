using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ImageEntityConfiguration : IEntityTypeConfiguration<ImageEntity>
{
    public void Configure(EntityTypeBuilder<ImageEntity> builder)
    {
        builder.ToTable("images");
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.Path).IsRequired().HasColumnName("path");
        builder.Property(x => x.FileName).IsRequired().HasColumnName("fileName");
        builder.Property(x => x.ProductCategoryId).IsRequired().HasColumnName("category_id");
        builder.Property(x => x.MimeType).IsRequired().HasColumnName("mime_type");

        builder.HasOne(x => x.ProductCategory).WithMany(x => x.Images)
            .HasForeignKey(x => x.ProductCategoryId).IsRequired()
            .OnDelete(DeleteBehavior.Cascade).HasConstraintName("category_constraint");
    }
}