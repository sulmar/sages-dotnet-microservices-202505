using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProductCatalog.Api.HealthChecks;

public class CustomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // TODO: Implement your custom health check logic here
        return Task.FromResult(HealthCheckResult.Unhealthy());
    }
}
