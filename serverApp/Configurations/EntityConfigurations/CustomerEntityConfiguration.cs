using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.UseTpcMappingStrategy();
        builder.ToTable("customers");
        
        builder.Property(x => x.CreditCardId).HasColumnName("credit_card_id");
        builder.HasOne<CreditCardEntity>().WithOne()
            .HasForeignKey<CustomerEntity>(x => x.CreditCardId).OnDelete(DeleteBehavior.SetNull);
    }
}