using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities.ProductCategories.Reviews;

namespace server_app.Infrastructure.EntityConfigurations;

public class ReviewEntityConfiguration : IEntityTypeConfiguration<ReviewEntity>
{
    public void Configure(EntityTypeBuilder<ReviewEntity> builder)
    {
        builder.ToTable("reviews");
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.Text).IsRequired().HasMaxLength(500).HasColumnName("text");
        builder.Property(x => x.Estimation).IsRequired().HasColumnName("estimation");

        builder.Property(x => x.CategoryId).IsRequired().HasColumnName("category_id");
        builder.Property(x => x.ReviewOwnerId).IsRequired().HasColumnName("owner_id");
        
        builder.HasOne(x => x.Category).WithMany(x => x.Reviews)
            .HasForeignKey(x => x.CategoryId).IsRequired();
        
        builder.HasOne(x => x.ReviewOwner).WithMany()
            .HasForeignKey(x => x.ReviewOwnerId).IsRequired();
    }
}