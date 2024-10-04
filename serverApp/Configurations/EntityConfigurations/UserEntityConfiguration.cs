using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.UseTpcMappingStrategy();
        //TPH - Table Per Hierarchy все свойство наследников будут как колонки ОДНОЙ таблицы
        //TPT - Table Per Type ребенок имеет отдельнуд таблицу, колонками будут создаваться только основываясь на его свойствах
        //Также создаеться общия таблица Users
        //TPC - Table Per Class тут же в отл от TPT даже те что наследуешь свойства будут как колонки одной таблицы
        
        //builder.HasKey(x => x.Id).HasName("user_id");
        //Делаем только для UserEntity Id колонку, иначе efcore орать будет хули родитель не имеет колонку
        //Ребенку ее переобределять нельзя, что пиздец кстати
        
        builder.Property(x => x.Name).HasMaxLength(25).IsRequired().HasColumnName("name");
        builder.Property(x => x.Email).HasMaxLength(50).IsRequired().HasColumnName("email");
        builder.Property(x => x.EmailVerify).IsRequired().HasColumnName("email_verify");
        builder.Property(x => x.PasswordHash).IsRequired().HasColumnName("password_hash");
    }
}