using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace HealthCheck.Server;

public class ICMPHealthCheck : IHealthCheck
{
    private readonly string _host ;
    private readonly int _healthyRoundtripTime = 300;

    public ICMPHealthCheck(string  host, int healthyRoundtrimTime)
    {
        _host = host;
        _healthyRoundtripTime = healthyRoundtrimTime;
    }
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(_host);
            switch(reply.Status)
            {
                case IPStatus.Success:
                    var msg =
                        $"ICMP to {_host} took {reply.RoundtripTime} ms.";
                    return (reply.RoundtripTime > _healthyRoundtripTime)
                        ? HealthCheckResult.Degraded(msg)
                        : HealthCheckResult.Healthy(msg);
                default:
                    var err =
                        $"ICMP to {_host} failed: {reply.Status}";
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
