using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApiOpenTelemetry.HealthCheks;

public class SampleHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        // if secods are even, then the health check is unhealthy
        if (DateTime.Now.Second % 2 == 0) isHealthy = false;

        if (isHealthy)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("A healthy result."));
        }

        return Task.FromResult(
            new HealthCheckResult(
                context.Registration.FailureStatus, "An unhealthy result."));
    }
}
