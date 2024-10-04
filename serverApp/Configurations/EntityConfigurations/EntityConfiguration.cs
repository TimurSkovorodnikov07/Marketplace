using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
    public void Configure(EntityTypeBuilder<Entity> builder)
    {
        builder.UseTpcMappingStrategy();
        
        builder.Property(x => x.Id).IsRequired().HasColumnName("id");
        builder.HasKey(x => x.Id);
        //БЛЯТЬ!!! Я думал но колонку сам создаст судя по HasKey, а HasName думал это имя, ОШИБАЛСЯ нахуй
    }
}