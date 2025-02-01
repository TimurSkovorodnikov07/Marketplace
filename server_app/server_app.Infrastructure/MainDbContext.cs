using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using server_app.Domain.Entities.ProductCategories;
using server_app.Domain.Entities.ProductCategories.DeliveryCompanies;
using server_app.Domain.Entities.ProductCategories.PurchasedProducts;
using server_app.Domain.Entities.ProductCategories.Ratings;
using server_app.Domain.Entities.ProductCategories.Reviews;
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
        //Efcore странный конечно, вобще очень
        //Было вобщем так, до этого у меня не было UserEntityConf. и он орал, типо CustomerEntity привязяться к UserEntity не может
        //или что то в этом роде, ну и вот, https://qna.habr.com/q/1371112 спасибо конечно мужику, очень помог
        //Ну и вобщем блять вопрос, с херали такая хуйня лишь с UserEntity? Почему он не орал на просто Entity
        //На всякий сделал EntityConf., так блять он начал кидать warn на то что чето не так с Entity, блять, а без конфига на Entity не кидает warn-ы
        //Ебанная параша
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
        //modelBuilder.ApplyConfiguration(new ImageEntityConfiguration()); Не юзаю тк все данные храняться уже в монго
    }

    public static ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(conf =>
    {
        conf.AddConsole();
        //For this method need loaded to the nuget package Microsoft.Extensions.Logging.Console
    });
}