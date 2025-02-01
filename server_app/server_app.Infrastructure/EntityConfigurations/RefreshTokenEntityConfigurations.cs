using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Users.Tokens;

namespace server_app.Infrastructure.EntityConfigurations;

public class RefreshTokenEntityConfigurations : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("refresh_tokens");
        builder.UseTpcMappingStrategy();
        
        
        builder.Property(x => x.TokenHash).IsRequired().HasColumnName("token_hash");
        builder.HasKey(x => x.TokenHash);
        builder.Property(x => x.UserId).IsRequired().HasColumnName("user_id");
        
        builder.HasOne<UserEntity>().WithOne().HasForeignKey<RefreshTokenEntity>(x => x.UserId)
            .IsRequired().OnDelete(DeleteBehavior.Cascade).HasConstraintName("user_constraint");
    }
}