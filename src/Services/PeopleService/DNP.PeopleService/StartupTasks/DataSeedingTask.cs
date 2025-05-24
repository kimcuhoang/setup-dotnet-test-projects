using DNP.PeopleService.Features.People.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Person = DNP.PeopleService.Features.People.Domain.Person;

namespace DNP.PeopleService.StartupTasks;
public class DataSeedingTask(ILogger<DataSeedingTask> logger, IServiceScopeFactory scopeFactory) : IStartupTask
{
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using var scope = scopeFactory.CreateAsyncScope();
        var serviceProvider = scope.ServiceProvider;
        
        try
        {
            var personRepository = serviceProvider.GetRequiredService<IPersonRepository>();
            await personRepository.Add(Person.Default, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding data.");
        }
    }
}