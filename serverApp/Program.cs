using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors((opts) =>
{
    opts.AddPolicy("defaultCorsPolicy", (builder) =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddAuthentication(auth =>
{
    var defaultAuthSceme = JwtBearerDefaults.AuthenticationScheme;
    
    auth.DefaultAuthenticateScheme = defaultAuthSceme;
    auth.DefaultChallengeScheme = defaultAuthSceme;
}).AddJwtBearer((opts) =>
{
    opts.RequireHttpsMetadata = false/*TODO true*/;

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
        [new OpenApiSecurityScheme {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" },
                Name = "Authorization"
            }] = new List<string>()
    });
});
builder.Services.AddControllers();
builder.Services.AddDbContext<MainDbContext>();

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

builder.Services.AddSingleton<ITokenNameInCookies>(jwtOptions);
builder.Services.AddSingleton<BaseEmailSenderService>();
builder.Services.AddSingleton<IHasher, HashingManagerService>();
builder.Services.AddSingleton<IHashVerify, HashingManagerService>();
builder.Services.AddSingleton(typeof(JwtService<>));

builder.Services.AddSingleton<ICodeCreator, CodeService>();
builder.Services.AddSingleton<IEmailSender, EmailSenderByYandexService>();
builder.Services.AddSingleton<IEmailVerify, EmailVerifyService>();
builder.Services.AddScoped<FileService>();

builder.Services.AddScoped<DeliveryCompanyService>();
builder.Services.AddScoped<RefreshTokenService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CreditCardService>();
builder.Services.AddScoped<ImagesService>();
builder.Services.AddScoped<ProductCategoryService>();

builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddScoped<AgeCheckHandler>();
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
    app.UseHttpsRedirection();
}
app.UseRouting();
app.UseCors("defaultCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();