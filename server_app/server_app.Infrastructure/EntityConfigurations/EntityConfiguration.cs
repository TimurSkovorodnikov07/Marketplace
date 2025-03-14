using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities;

namespace server_app.Infrastructure.EntityConfigurations;

public class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
    public void Configure(EntityTypeBuilder<Entity> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.Property(x => x.Id).IsRequired().HasColumnName("id");
        builder.HasKey(x => x.Id);
    }
}
