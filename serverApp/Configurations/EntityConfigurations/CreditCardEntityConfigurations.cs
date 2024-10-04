using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CreditCardEntityConfigurations : IEntityTypeConfiguration<CreditCardEntity>
{
    public void Configure(EntityTypeBuilder<CreditCardEntity> builder)
    {
        builder.ToTable("credit_cards");
        
        builder.Property(x => x.Number).IsRequired().HasMaxLength(20).HasColumnName("number");
        builder.Property(x => x.Many).IsRequired().HasColumnName("many");
        builder.Property(x => x.Type).IsRequired().HasColumnName("type");
        builder.Property(x => x.OwnerId).HasColumnName("owner_id").IsRequired();
        
        builder.HasOne<CustomerEntity>().WithOne().IsRequired()
            .HasForeignKey<CreditCardEntity>(x => x.OwnerId).OnDelete(DeleteBehavior.Cascade);
    }
}