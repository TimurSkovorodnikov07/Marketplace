using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.ValueObjects;

namespace server_app.Infrastructure.EntityConfigurations;

public class DeliveryCompanyEntityConfiguration : IEntityTypeConfiguration<DeliveryCompanyEntity>
{
    public void Configure(EntityTypeBuilder<DeliveryCompanyEntity> builder)
    {
        //Default delivery companies(знаю не правильынй подход, но смысл создавать отдельный api как в реале и оттуда уже брать данные компаний):

        //Почему важно создавать собственные Guid:
        //https://stackoverflow.com/questions/79312974/how-to-fix-database-update-pendingmodelchangeswarning-error
        var firstId = new Guid("ab977dee-7ba0-4c8e-9700-763d702977a0");
        var secondId = new Guid("ab977dee-7ba0-4c8e-9700-763d702977a1");
        var thirdId = new Guid("ab977dee-7ba0-4c8e-9700-763d702977a2");
        var fourthId = new Guid("ab977dee-7ba0-4c8e-9700-763d702977a5");

        //О том как я просрал 3-4 часа жизни: https://qna.habr.com/q/1389286
        //Я вот шаманил, не понимал в чем дело, поэтому 12+ миграций сделал, самое угарное что я не дадумался за это время взять и ТОЧНО убедиться что 
        //efcore "сохраняет не так", банально можно было зайти посмотреть данные базы 
        var companies = new[]
        {
            new DeliveryCompanyEntity
            {
                Id = firstId, Name = "DeliveryCompanyNum 1", Description = "Description 1",
                WebSite = new WebSiteValueObject { WebSiteValue = "https://helloworld.gov/" },
                PhoneNumber = new PhoneNumberValueObject { Number = "+7 888 032 0324" }
            },
            new DeliveryCompanyEntity
            {
                Id = secondId, Name = "Transporter company", Description = "Blahblahblah",
                WebSite = new WebSiteValueObject { WebSiteValue = "https://transporter.com/" },
                PhoneNumber = new PhoneNumberValueObject { Number = "+6 533 003 0002" }
            },
            new DeliveryCompanyEntity
            {
                Id = thirdId, Name = "Some Dodecahedron", Description = "Blah blah blah",
                WebSite = new WebSiteValueObject { WebSiteValue = "https://dodecahedron.org/" },
                PhoneNumber = new PhoneNumberValueObject { Number = "+7 007 942 2390" }
            },
            new DeliveryCompanyEntity
            {
                Id = fourthId, Name = "Some DC", Description = "Blah123 blah blah...",
                WebSite = new WebSiteValueObject { WebSiteValue = "https://metanit.com/sharp/aspnet6/" },
                PhoneNumber = new PhoneNumberValueObject { Number = "+1 117 955 0000" }
            }
        };
        if (companies.Any(x => x == null))
            throw new NullReferenceException("There is company or companies that are null");


        builder.UseTpcMappingStrategy();
        builder.ToTable("delivery_companies");

        builder.Property(x => x.Name).HasMaxLength(20).IsRequired().HasColumnName("name");
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired().HasColumnName("description");

        builder.OwnsOne(x => x.WebSite, property =>
        {
            property.Property(x => x.WebSiteValue).IsRequired()
                .HasMaxLength(255).HasColumnName("website");

            property.HasIndex(x => x.WebSiteValue).IsUnique();
        });
        builder.OwnsOne(x => x.PhoneNumber, property =>
        {
            property.Property(x => x.Number)
                .IsRequired().HasMaxLength(20).HasColumnName("phone_number");

            property.HasIndex(x => x.Number).IsUnique();
        });


        builder.HasData(companies.Select(c => new
        {
            c.Id,
            c.Name,
            c.Description
        }));
        builder.OwnsOne(x => x.WebSite).HasData(companies.Select(c => new
        {
            DeliveryCompanyEntityId = c.Id,
            WebSiteValue = c.WebSite.WebSiteValue
        }));
        builder.OwnsOne(x => x.PhoneNumber).HasData(companies.Select(c => new
        {
            DeliveryCompanyEntityId = c.Id,
            Number = c.PhoneNumber.Number
        }));
        //Efcore по дефолту орал на свойство PhoneNumber после того как я хотел мигр сдлеать(Unable to create a 'DbContext' of type 'MainDbContext'. The exception 'The seed entity for entity type 'DeliveryCompanyEntity' with the key value 'Id:74f19594-95c6-4612-a46c-27e3f56059c1' cannot be added because it has the navigation 'PhoneNumber' set. To seed relationships, add the entity seed to 'PhoneNumberValueObject' and specify the foreign key values {'DeliveryCompanyEntityId'}.' was thrown while attempting to create an instance. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728)
        //В итоге решение просто добавить под OwnsOne нужные данные как анон типы:
        //https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding#model-seed-data 


        builder.HasIndex(x => x.Name).IsUnique();
    }
}