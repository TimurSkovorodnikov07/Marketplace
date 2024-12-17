using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RatingEntityConfiguration : IEntityTypeConfiguration<RatingEntity>
{
    public void Configure(EntityTypeBuilder<RatingEntity> builder)
    {
        builder.ToTable("ratings");
        builder.Property(x => x.ProductCategoryId).IsRequired().HasColumnName("category_id");
        builder.HasKey(x => x.ProductCategoryId);
        builder.Property(x => x.TotalRating).IsRequired().HasColumnName("total_rating");

        builder.HasOne(x => x.ProductCategory).WithOne()
            .HasForeignKey<RatingEntity>(x => x.ProductCategoryId).IsRequired();
    }
}