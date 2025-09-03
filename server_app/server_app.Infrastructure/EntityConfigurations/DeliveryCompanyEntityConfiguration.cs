using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.ValueObjects;

namespace server_app.Infrastructure.EntityConfigurations;

public class DeliveryCompanyEntityConfiguration : IEntityTypeConfiguration<DeliveryCompanyEntity>
{
    public void Configure(EntityTypeBuilder<DeliveryCompanyEntity> builder)
    {
        var firstId = new Guid("ab977dee-7ba0-4c8e-9700-763d702977a0");
        var secondId = new Guid("ab977dee-7ba0-4c8e-9700-763d702977a1");
        var thirdId = new Guid("ab977dee-7ba0-4c8e-9700-763d702977a2");
        var fourthId = new Guid("ab977dee-7ba0-4c8e-9700-763d702977a5");

        var companies = new[]
        {
            new DeliveryCompanyEntity
            {
                Id = firstId,
                Name = "DeliveryCompanyNum 1",
                Description = "Description 1",
                WebSite = new WebSiteValueObject { WebSiteValue = "https://helloworld.gov/" },
                PhoneNumber = new PhoneNumberValueObject { Number = "+7 888 032 0324" },
            },
            new DeliveryCompanyEntity
            {
                Id = secondId,
                Name = "Transporter company",
                Description = "Blahblahblah",
                WebSite = new WebSiteValueObject { WebSiteValue = "https://transporter.com/" },
                PhoneNumber = new PhoneNumberValueObject { Number = "+6 533 003 0002" },
            },
            new DeliveryCompanyEntity
            {
                Id = thirdId,
                Name = "Some Dodecahedron",
                Description = "Blah blah blah",
                WebSite = new WebSiteValueObject { WebSiteValue = "https://dodecahedron.org/" },
                PhoneNumber = new PhoneNumberValueObject { Number = "+7 007 942 2390" },
            },
            new DeliveryCompanyEntity
            {
                Id = fourthId,
                Name = "Some DC",
                Description = "Blah123 blah blah...",
                WebSite = new WebSiteValueObject
                {
                    WebSiteValue = "https://metanit.com/sharp/aspnet6/",
                },
                PhoneNumber = new PhoneNumberValueObject { Number = "+1 117 955 0000" },
            },
        };
        if (companies.Any(x => x == null))
            throw new NullReferenceException("There is company or companies that are null");

        builder.UseTpcMappingStrategy();
        builder.ToTable("delivery_companies");

        builder.Property(x => x.Name).HasMaxLength(20).IsRequired().HasColumnName("name");
        builder
            .Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired()
            .HasColumnName("description");

        builder.OwnsOne(
            x => x.WebSite,
            property =>
            {
                property
                    .Property(x => x.WebSiteValue)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("website");

                property.HasIndex(x => x.WebSiteValue).IsUnique();
            }
        );
        builder.OwnsOne(
            x => x.PhoneNumber,
            property =>
            {
                property
                    .Property(x => x.Number)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("phone_number");

                property.HasIndex(x => x.Number).IsUnique();
            }
        );

        builder.HasData(
            companies.Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
            })
        );
        builder
            .OwnsOne(x => x.WebSite)
            .HasData(
                companies.Select(c => new
                {
                    DeliveryCompanyEntityId = c.Id,
                    WebSiteValue = c.WebSite.WebSiteValue,
                })
            );
        builder
            .OwnsOne(x => x.PhoneNumber)
            .HasData(
                companies.Select(c => new
                {
                    DeliveryCompanyEntityId = c.Id,
                    Number = c.PhoneNumber.Number,
                })
            );

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
