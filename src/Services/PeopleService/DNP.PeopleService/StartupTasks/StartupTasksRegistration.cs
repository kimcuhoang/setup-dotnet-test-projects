namespace DNP.PeopleService.StartupTasks;

internal static class StartupTasksRegistration
{
    public static WebApplicationBuilder AddStartupTasks(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddTransient<IStartupTask, DatabaseMigrationTask>()
            .AddTransient<IStartupTask, DataSeedingTask>()
            .AddHostedService<StartupTasksRunner>();

        return builder;
    }
}
