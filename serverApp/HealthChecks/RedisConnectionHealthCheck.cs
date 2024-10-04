using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

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
        var _connectionInfo = StackExchange.Redis.ConnectionMultiplexer.Connect(_connectionStr);
        var ping = await _connectionInfo.GetDatabase().PingAsync();

        var seconds = ping.Seconds;
            return (seconds > _healthOptions.ConnectionTakesNoMoreSeconds)
                ? HealthCheckResult.Degraded($"Degraded. Connected for: {seconds}") 
                : HealthCheckResult.Healthy($"Connection is OK. Connected for: {seconds}");
    }
}