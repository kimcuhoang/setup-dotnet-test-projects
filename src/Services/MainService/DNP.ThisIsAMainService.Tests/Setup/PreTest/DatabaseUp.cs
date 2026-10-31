using Microsoft.Extensions.Hosting;

namespace DNP.ThisIsAMainService.Tests.Setup.PreTest;

/// <summary>
/// Implements <see cref="IHostedService"/> rather than <see cref="BackgroundService"/> so that
/// host startup waits for the migrations, otherwise tests can run against an unmigrated database.
/// </summary>
public class DatabaseUp(IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(cancellationToken);
        if (pendingMigrations.Any())
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
