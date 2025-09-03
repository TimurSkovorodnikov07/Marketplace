using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities.ProductCategories.Ratings;

namespace server_app.Infrastructure.EntityConfigurations;

public class RatingFromCustomerEntityConfigurations : IEntityTypeConfiguration<RatingFromCustomerEntity>
{
    public void Configure(EntityTypeBuilder<RatingFromCustomerEntity> builder)
    {
        builder.ToTable("rating_from_customers");
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.CustomerId).IsRequired().HasColumnName("customer_id");
        builder.Property(x => x.CommonRatingId).IsRequired().HasColumnName("common_ratting_id");
        builder.Property(x => x.Rating).IsRequired().HasColumnName("ratting");

        
        builder.HasOne(x => x.Customer).WithMany()
            .HasForeignKey(x => x.CustomerId).IsRequired();
        
        builder.HasOne(x => x.CommonRating).WithMany(x => x.RattingFromCustomers)
            .HasForeignKey(x => x.CommonRatingId).IsRequired();
    }
}