using Microsoft.Extensions.Hosting;

namespace DNP.ThisIsAMainServices.TestsV1.Setup.PreTest;

public class DatabaseUp(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(stoppingToken);
        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync(stoppingToken);
        }
    }
}