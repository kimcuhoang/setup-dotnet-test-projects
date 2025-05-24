namespace DNP.PeopleService.StartupTasks;
public class StartupTasksRunner(ILogger<StartupTasksRunner> logger, IServiceScopeFactory serviceScopeFactory): IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var startupTasks = serviceScopeFactory.CreateScope().ServiceProvider.GetServices<IStartupTask>().ToList();
        foreach (var startupTask in startupTasks)
        {
            try
            {
                await startupTask.ExecuteAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing startup task {TaskName}", startupTask.GetType().Name);
            }
        }
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
