using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace HealthCheck.Server;

public class ICMPHealthCheck : IHealthCheck
{
    private readonly string Host = $"10.0.0.0";
    private readonly int HealthyRoundtripTime = 300;

    public ICMPHealthCheck(string  host, int healthyRoundtrimTime)
    {
        Host = host;
        HealthyRoundtripTime = healthyRoundtrimTime;
    }
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
                    var msg =
                        $"ICMP to {Host} took {reply.RoundtripTime} ms.";
                    return (reply.RoundtripTime > HealthyRoundtripTime)
                        ? HealthCheckResult.Degraded(msg)
                        : HealthCheckResult.Healthy(msg);
                default:
                    var err =
                        $"ICMP to {Host} failed: {reply.Status}";
                    return HealthCheckResult.Unhealthy();
            }
        }
        catch (Exception e)
        {
            var err = $"ICMP failed: {e.Message}";
            return HealthCheckResult.Unhealthy(err);
        }
    }
}