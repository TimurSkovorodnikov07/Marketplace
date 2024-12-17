using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

//$sudo docker run -e ASPNETCORE_ENVIRONMENT=Development -v /root/.microsoft/usersecrets skovorodnikovtimur07/marketplace-server-app
var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var jwtOptions =
    builder.Configuration.GetRequiredSection("UserSecrets:Jwt").Get<JwtOptions>();
builder.Services.AddHealthChecks()
    .AddCheck<RedisConnectionHealthCheck>(nameof(RedisConnectionHealthCheck));
builder.Services.AddStackExchangeRedisCache((opts) =>
{
    string connectionStr = builder.Configuration["UserSecrets:RedisConnectionStr"];

    if (string.IsNullOrEmpty(connectionStr))
        throw new NullReferenceException("Redis connection string is empty");

    opts.Configuration = connectionStr;
});
builder.Services.AddAutoMapper(typeof(MainMapperProfile));
builder.Services.AddCors((opts) =>
{
    opts.AddDefaultPolicy((corsPolicyBuilder) =>
    {
        corsPolicyBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders(BaseLoginController.AccountIsConfirmedHeaderType,
                SellerController.GetForOwnerHeaderType,
                ProductsController.IsForOwnerHeaderType,
                ProductsController.CategoriesMaxNumberHeaderType,
                ProductsController.IsBoughtHeaderType,
                ProductsController.GetIsBoughtRequestHeaderType,
                ProductsController.PurchasedProductsMaxNumberHeaderType,
                ReviewController.GetReviewsMaxCount);
    });
});
builder.Services.AddAuthentication(auth =>
{
    var defaultAuthScheme = JwtBearerDefaults.AuthenticationScheme;

    auth.DefaultAuthenticateScheme = defaultAuthScheme;
    auth.DefaultChallengeScheme = defaultAuthScheme;
}).AddJwtBearer((opts) =>
{
    opts.RequireHttpsMetadata = false /*TODO true*/;

    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAlgorithms = new List<string> { jwtOptions.AlgorithmForAccessToken },
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = jwtOptions.GetAccessSymmetricSecurityKey(),

        ValidateLifetime = true,
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
        {
            if (expires != null)
                return expires.Value > DateTime.UtcNow;

            return false;
        },
    };
});
builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    o.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Description = "Basic auth added to authorization header",
        Name = "Authorization",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" },
            Name = "Authorization"
        }] = new List<string>(),
    });
});
builder.Services.AddDbContext<MainDbContext>(optionsBuilder =>
{
    // Unable to create a 'DbContext' of type ''. The exception 'Unable to resolve service for type 'Microsoft.Extensions.Configuration.IConfiguration' while attempting to activate 'MainDbContext'.' was thrown while attempting to create an instance. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
    // Ебанный efcore опять радует своими ебучими ошибками
    // Потому решил не юзать OnConfiguring, тк а как же мне нахуй брать сервисы тогда, пиздец
    var pgConnectionStr = builder.Configuration["UserSecrets:PostgresConnectionStr"];

    if (string.IsNullOrWhiteSpace(pgConnectionStr))
        throw new NullReferenceException("No PostgresConnectionStr");
    optionsBuilder
        .UseNpgsql(pgConnectionStr)
        .UseLoggerFactory(MainDbContext.CreateLoggerFactory())
        .EnableSensitiveDataLogging();
});
builder.Services.AddControllers();

#region addServices

//Add IOptions
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetRequiredSection("UserSecrets:Jwt"));
builder.Services.Configure<VerfiyCodeOptions>(
    builder.Configuration.GetRequiredSection("VerifyCode"));
builder.Services.Configure<HealthOptions>(
    builder.Configuration.GetRequiredSection("Health"));
builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetRequiredSection("UserSecrets:Email"));
builder.Services.Configure<RatingForceOptions>(
    builder.Configuration.GetRequiredSection("RattingForce"));


builder.Services.AddSingleton<ITokenNameInCookies>(jwtOptions);
builder.Services.AddSingleton<BaseEmailSenderService>();
builder.Services.AddSingleton<IHasher, HashingManagerService>();
builder.Services.AddSingleton<IHashVerify, HashingManagerService>();

builder.Services.AddSingleton<ICodeCreator, CodeService>();
builder.Services.AddSingleton<IEmailSender, EmailSenderByYandexService>();
builder.Services.AddSingleton<IEmailVerify, EmailVerifyService>();

builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<DeliveryCompanyService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UserEntityService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<CreditCardService>();
builder.Services.AddScoped<ImagesService>();
builder.Services.AddScoped<RatingService>();
builder.Services.AddScoped<ProductCategoryService>();
builder.Services.AddScoped<ReviewService>();

builder.Services.AddScoped<ValidationFilter>();
//builder.Services.AddScoped<AgeCheckHandler>();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseGlobalExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//Там был срачь с сохранением колонки с DateTime, добавил строчку выше ^

app.Run();