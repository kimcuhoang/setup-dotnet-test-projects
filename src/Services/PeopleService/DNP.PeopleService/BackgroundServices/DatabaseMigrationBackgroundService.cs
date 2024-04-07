
using Microsoft.EntityFrameworkCore;

namespace DNP.PeopleService.BackgroundServices;

public class DatabaseMigrationBackgroundService(ILogger<DatabaseMigrationBackgroundService> logger, IServiceScopeFactory scopeFactory) : IHostedService
{
    private readonly ILogger<DatabaseMigrationBackgroundService> _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateAsyncScope();
        var serviceProvider = scope.ServiceProvider;
        var dbContext = serviceProvider.GetRequiredService<DbContext>();

        var database = dbContext.Database;
        var pendingChanges = await database.GetPendingMigrationsAsync();

        if (!pendingChanges.Any())
        {
            this._logger.LogWarning("Nothing new. Database is up to date !!!!");
            return;
        }

        await database.MigrateAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
    }
}
