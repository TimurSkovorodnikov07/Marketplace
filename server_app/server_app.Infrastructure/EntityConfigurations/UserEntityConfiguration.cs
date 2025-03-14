using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace server_app.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.UseTpcMappingStrategy();
        //TPH - Table Per Hierarchy все свойство наследников будут как колонки ОДНОЙ таблицы
        //TPT - Table Per Type ребенок имеет отдельнуд таблицу, колонками будут создаваться только основываясь на его свойствах
        //Также создаеться общия таблица Users
        //TPC - Table Per Class тут же в отл от TPT даже те что наследуешь свойства будут как колонки одной таблицы
        
        builder.Property(x => x.Name).HasMaxLength(25).IsRequired().HasColumnName("name");
        builder.Property(x => x.Email).HasMaxLength(50).IsRequired().HasColumnName("email");
        builder.Property(x => x.EmailVerify).IsRequired().HasColumnName("email_verify");
        builder.Property(x => x.PasswordHash).IsRequired().HasColumnName("password_hash");
    }
}