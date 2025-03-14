using server_app.Application.Abstractions.EmailSend;
using server_app.Application.Abstractions.Hashing;
using server_app.Application.Options;
using server_app.Application.Repositories;
using server_app.Application.Services;
using server_app.Application.Services.MailServices;
using server_app.Infrastructure;
using server_app.Infrastructure.Repositories;
using server_app.Infrastructure.Repositories.ProductCategories;
using server_app.Infrastructure.Repositories.Users;
using server_app.Presentation.Filters;

namespace server_app.Presentation;

public static class DependenciesInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration,
        JwtOptions jwtOptions
    )
    {
        services.AddOptionsServices(configuration);

        services.AddSingleton<IMongoDb, MongoDb>();
        services.AddSingleton<ITokenNameInCookies>(jwtOptions);
        services.AddSingleton<BaseEmailSenderService>();
        services.AddSingleton<IHasher, HashingManagerService>();
        services.AddSingleton<IHashVerify, HashingManagerService>();

        services.AddSingleton<ICodeCreator, CodeService>();
        services.AddSingleton<IEmailSender, EmailSenderByYandexService>();
        services.AddSingleton<IEmailRepository, EmailRepository>();

        services.AddScoped<IDeliveryCompanyRepository, DeliveryCompanyRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<JwtService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICreditCardRepository, CreditCardRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();

        services.AddScoped<IRatingService, RatingRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IReviewsRepository, ReviewsRepository>();

        services.AddFilterServices(configuration);

        return services;
    }

    private static IServiceCollection AddOptionsServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MongoDbOptions>(configuration.GetRequiredSection("UserSecrets:MongoDb"));
        services.Configure<JwtOptions>(configuration.GetRequiredSection("UserSecrets:Jwt"));
        services.Configure<VerfiyCodeOptions>(configuration.GetRequiredSection("VerifyCode"));
        services.Configure<HealthOptions>(configuration.GetRequiredSection("Health"));
        services.Configure<EmailOptions>(configuration.GetRequiredSection("UserSecrets:Email"));
        services.Configure<RatingForceOptions>(configuration.GetRequiredSection("RattingForce"));

        return services;
    }

    private static IServiceCollection AddFilterServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ValidationFilter>();
        return services;
    }
}
