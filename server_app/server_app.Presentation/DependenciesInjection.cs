using MongoDB.Driver;
using server_app.Application.Abstractions.EmailSend;
using server_app.Application.Abstractions.Hashing;
using server_app.Application.MongoClient;
using server_app.Application.Services;
using server_app.Application.Services.EntitiesServices;
using server_app.Application.Services.EntitiesServices.Interfaces;
using server_app.Application.Services.FileServices;
using server_app.Application.Services.MailServices;
using server_app.Domain.Model.Options;
using server_app.Presentation.Filters;

namespace server_app.Presentation;

public static class DependenciesInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        IConfiguration configuration, JwtOptions jwtOptions)
    {
        services.AddOptionsServices(configuration);
        
        
        services.AddSingleton<IMongoDbClient, MongoDbClient>();
        services.AddSingleton<ITokenNameInCookies>(jwtOptions);
        services.AddSingleton<BaseEmailSenderService>();
        services.AddSingleton<IHasher, HashingManagerService>();
        services.AddSingleton<IHashVerify, HashingManagerService>();

        services.AddSingleton<ICodeCreator, CodeService>();
        services.AddSingleton<IEmailSender, EmailSenderByYandexService>();
        services.AddSingleton<IEmailVerify, EmailVerifyService>();

        services.AddScoped<ImageMongoDbService>();
        services.AddScoped<IDeliveryCompanyService, DeliveryCompanyService>();
        services.AddScoped<ISellerService, SellerService>();

        services.AddScoped<ICustomerService, CustomerService>();

        services.AddScoped<JwtService>();

        services.AddScoped<UserEntityService>();

        services.AddScoped<RefreshTokenService>();

        services.AddScoped<CreditCardService>();

        services.AddScoped<IImageEntityService, ImageEntityService>();

        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<ReviewService>();

        services.AddFilterServices(configuration);
        //builder.Services.AddScoped<AgeCheckHandler>();

        return services;
    }

    private static IServiceCollection AddOptionsServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbOptions>(
            configuration.GetRequiredSection("UserSecrets:MongoDb"));
        services.Configure<JwtOptions>(
            configuration.GetRequiredSection("UserSecrets:Jwt"));
        services.Configure<VerfiyCodeOptions>(
            configuration.GetRequiredSection("VerifyCode"));
        services.Configure<HealthOptions>(
            configuration.GetRequiredSection("Health"));
        services.Configure<EmailOptions>(
            configuration.GetRequiredSection("UserSecrets:Email"));
        services.Configure<RatingForceOptions>(
            configuration.GetRequiredSection("RattingForce"));

        return services;
    }

    private static IServiceCollection AddFilterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ValidationFilter>();
        return services;
    }
}