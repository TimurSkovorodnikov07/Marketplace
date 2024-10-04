using Microsoft.EntityFrameworkCore;

public class MainDbContext : DbContext
{
    public MainDbContext(IConfiguration conf,
        ILogger<MainDbContext> logger)
    {
        _logger = logger;
        _pgConnectionStr = conf["UserSecrets:PostgresConnectionStr"];

        if (string.IsNullOrWhiteSpace(_pgConnectionStr))
            throw new NullReferenceException("No PostgresConnectionStr");
    }

    
    public DbSet<CustomerEntity> Customers { get; set; } = null;
    public DbSet<CreditCardEntity> CreditCards { get; set; } = null;
    public DbSet<SellerEntity> Sellers { get; set; } = null;
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; } = null;
    public DbSet<ProductCategoryEntity> ProductsCategory { get; set; } = null;
    public DbSet<PurchasedProductEntity> PurchasedProducts { get; set; } = null;
    public DbSet<DeliveryCompanyEntity> Companies { get; set; } = null;
    public DbSet<ImageEntity> Images { get; set; } = null;

    private readonly string _pgConnectionStr;
    private readonly ILogger<MainDbContext> _logger;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(_pgConnectionStr)
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }

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
        modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfigurations());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CreditCardEntityConfigurations());
        modelBuilder.ApplyConfiguration(new CustomerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SellerEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ImageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryEntityConfigurations());
        modelBuilder.ApplyConfiguration(new PurchasedProductEntityConfigurations());
    }

    private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(conf => { conf.AddConsole(); });
}