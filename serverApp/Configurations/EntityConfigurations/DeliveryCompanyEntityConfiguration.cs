using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DeliveryCompanyEntityConfiguration : IEntityTypeConfiguration<DeliveryCompanyEntity>
{
    public void Configure(EntityTypeBuilder<DeliveryCompanyEntity> builder)
    {
        builder.UseTpcMappingStrategy();
        builder.ToTable("delivery_companies");
        
        builder.Property(x => x.Name).HasMaxLength(20).IsRequired().HasColumnName("name");
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired().HasColumnName("description");
        builder.Property(x => x.WebSite).IsRequired().HasColumnName("website");
        builder.OwnsOne(x => x.PhoneNumber, property =>
        {
            property.Property(x => x.Number)
                .IsRequired().HasMaxLength(20).HasColumnName("phone_number");
            
            property.HasIndex(x => x.Number).IsUnique();
        });
        
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.WebSite).IsUnique();
    }
}