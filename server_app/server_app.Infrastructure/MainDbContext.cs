using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.PurchasedProducts;
using server_app.Domain.Entities.ProductCategories.Ratings;
using server_app.Domain.Entities.ProductCategories.Reviews;
using server_app.Domain.Entities.ProductCategories.ValueObjects;
using server_app.Domain.Entities.Users.CreditCard;
using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Entities.Users.Seller;
using server_app.Domain.Model.Dtos;
using server_app.Domain.Users.Tokens;
using server_app.Infrastructure.EntityConfigurations;

namespace server_app.Infrastructure;

public class MainDbContext(DbContextOptions optionsBuilder) : DbContext(optionsBuilder)
{
    public DbSet<CustomerEntity> Customers { get; set; } = null!;
    public DbSet<CreditCardEntity> CreditCards { get; set; } = null!;
    public DbSet<SellerEntity> Sellers { get; set; } = null!;
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null!;
    public DbSet<ProductCategoryEntity> ProductsCategories { get; set; } = null!;
    public DbSet<PurchasedProductEntity> PurchasedProducts { get; set; } = null!;
    public DbSet<DeliveryCompanyEntity> Companies { get; set; } = null!;
    public DbSet<RatingEntity> Rattings { get; set; } = null!;
    public DbSet<RatingFromCustomerEntity> RattingFromCustomers { get; set; } = null!;
    public DbSet<ReviewEntity> Reviews { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EntityConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryCompanyEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SellerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CreditCardEntityConfigurations());
        modelBuilder.ApplyConfiguration(new ProductCategoryEntityConfigurations());
        modelBuilder.ApplyConfiguration(new ReviewEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PurchasedProductEntityConfigurations());
        modelBuilder.ApplyConfiguration(new RatingEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RatingFromCustomerEntityConfigurations());
        modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfigurations());
    }

    public static ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(conf =>
    {
        conf.AddConsole();
    });
}