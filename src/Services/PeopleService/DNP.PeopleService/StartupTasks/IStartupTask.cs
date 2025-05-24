namespace DNP.PeopleService.StartupTasks;
public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
