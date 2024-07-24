using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace HealthCheck.Server;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}


public class ICMPHealthCheck : IHealthCheck
{
    private readonly string Host = $"10.0.0.0";
    private readonly int HealthyRoundtripTime = 300;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(Host);
            switch(reply.Status)
            {
                case IPStatus.Success:
                    return (reply.RoundtripTime > HealthyRoundtripTime)
                        ? HealthCheckResult.Degraded()
                        : HealthCheckResult.Healthy();
                default:
                    return HealthCheckResult.Unhealthy();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return HealthCheckResult.Unhealthy();
        }
    }
}