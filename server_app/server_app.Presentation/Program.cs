using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using server_app.Application.Options;
using server_app.Domain;
using server_app.Infrastructure;
using server_app.Presentation;
using server_app.Presentation.Controllers;
using server_app.Presentation.Controllers.UserControllers;
using server_app.Presentation.Extensions;
using server_app.Presentation.MapperProfiles;
using server_app.Presentation.Middlewares;
using server_app.Presentation.Middlewares.HealthChecks;


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
AddMongoConfigurationExtensions.AddMongoConfiguration();
builder.Services.AddDbContext<MainDbContext>(optionsBuilder =>
{
    var pgConnectionStr = builder.Configuration["UserSecrets:PostgresConnectionStr"];

    if (string.IsNullOrWhiteSpace(pgConnectionStr))
        throw new NullReferenceException("No PostgresConnectionStr");

    optionsBuilder
        .UseNpgsql(pgConnectionStr)
        .UseLoggerFactory(MainDbContext.CreateLoggerFactory())
        .EnableSensitiveDataLogging();
});
builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration, jwtOptions);

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
app.ApplyMigration();
//for the efcore use migrations in a docker container, without this postgres will be without changed tables
//https://youtube.com/watch?v=WQFx2m5Ub9M

app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//Там был срачь с сохранением колонки с DateTime, добавил потому строчку выше ^

app.Run();