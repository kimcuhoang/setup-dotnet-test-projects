using Microsoft.EntityFrameworkCore;

namespace DNP.PeopleService.StartupTasks;

public class DatabaseMigrationTask(ILogger<DatabaseMigrationTask> logger, IServiceScopeFactory scopeFactory) : IStartupTask
{
    private readonly ILogger<DatabaseMigrationTask> _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using var scope = this._scopeFactory.CreateAsyncScope();
        var serviceProvider = scope.ServiceProvider;
        var dbContext = serviceProvider.GetRequiredService<DbContext>();

        var database = dbContext.Database;
        var pendingChanges = await database.GetPendingMigrationsAsync(cancellationToken);

        if (!pendingChanges.Any())
        {
            this._logger.LogWarning("Nothing new. Database is up to date !!!!");
            return;
        }

        await database.MigrateAsync(cancellationToken);
    }
}
