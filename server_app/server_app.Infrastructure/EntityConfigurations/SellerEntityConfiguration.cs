using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities.Users.Seller;

namespace server_app.Infrastructure.EntityConfigurations;

public class SellerEntityConfiguration : IEntityTypeConfiguration<SellerEntity>
{
    public void Configure(EntityTypeBuilder<SellerEntity> builder)
    {
        builder.ToTable("sellers");
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.Description).IsRequired().HasMaxLength(125).HasColumnName("description");
    }
}