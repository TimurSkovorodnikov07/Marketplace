using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using server_app.Application.Options;

namespace server_app.Presentation.Middlewares.HealthChecks;

public class RedisConnectionHealthCheck(
    ILogger<RedisConnectionHealthCheck> logger,
    IOptions<HealthOptions> options,
    IConfiguration conf)
    : IHealthCheck
{
    private readonly ILogger<RedisConnectionHealthCheck> _logger = logger;
    private readonly HealthOptions _healthOptions = options.Value;
    private readonly string _connectionStr = conf["UserSecrets:RedisConnectionStr"];

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var connectionInfo = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(_connectionStr);
        var ping = await connectionInfo.GetDatabase().PingAsync();

        var seconds = ping.Seconds;
        return (seconds > _healthOptions.ConnectionTakesNoMoreSeconds)
            ? HealthCheckResult.Degraded($"Degraded. Connected for: {seconds}") 
            : HealthCheckResult.Healthy($"Connection is OK. Connected for: {seconds}");
    }
}