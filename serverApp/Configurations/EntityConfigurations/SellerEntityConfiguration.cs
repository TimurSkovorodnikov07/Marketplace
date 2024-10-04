using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SellerEntityConfiguration : IEntityTypeConfiguration<SellerEntity>
{
    public void Configure(EntityTypeBuilder<SellerEntity> builder)
    {
        builder.ToTable("sellers");
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.Description).IsRequired().HasMaxLength(500).HasColumnName("description");
    }
}