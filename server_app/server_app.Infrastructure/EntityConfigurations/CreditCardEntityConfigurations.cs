using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities.Users.CreditCard;

namespace server_app.Infrastructure.EntityConfigurations;

public class CreditCardEntityConfigurations : IEntityTypeConfiguration<CreditCardEntity>
{
    public void Configure(EntityTypeBuilder<CreditCardEntity> builder)
    {
        builder.ToTable("credit_cards");
        
        builder.Property(x => x.NumberHash).IsRequired().HasColumnName("number_hash");
        builder.Property(x => x.Many).IsRequired().HasColumnName("many");
        builder.Property(x => x.Type).IsRequired()
            .HasConversion(
                v => v.ToString(),
                v => (CreditCardType)Enum.Parse(typeof(CreditCardType), v))
            .HasColumnName("type");
        
        builder.Property(x => x.OwnerId)
            .IsRequired().HasColumnName("owner_id");
        
        builder.HasOne(x => x.Owner).WithOne(x => x.CreditCard)
            .HasForeignKey<CreditCardEntity>(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_customers_credit_cards");
    }
}