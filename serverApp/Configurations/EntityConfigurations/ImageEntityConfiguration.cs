using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ImageEntityConfiguration : IEntityTypeConfiguration<ImageEntity>
{
    public void Configure(EntityTypeBuilder<ImageEntity> builder)
    {
        builder.ToTable("images");
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.Path).IsRequired().HasColumnName("path");
        builder.Property(x => x.ProductId).IsRequired().HasColumnName("product_id");

        builder.HasOne(x => x.ProductCategory).WithMany(x => x.Images)
            .HasForeignKey(x => x.ProductId).IsRequired()
            .OnDelete(DeleteBehavior.Cascade).HasConstraintName("product_constraint");
    }
}