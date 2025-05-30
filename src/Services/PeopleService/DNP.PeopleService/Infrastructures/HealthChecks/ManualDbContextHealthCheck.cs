using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DNP.PeopleService.Infrastructures.HealthChecks;

public class ManualDbContextHealthCheck(DbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var check = await dbContext.Database.CanConnectAsync(cancellationToken);
            return check ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(exception: exception);
        }
    }
}
