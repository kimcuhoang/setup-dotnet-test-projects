using DNP.ThisIsAMainService.Shared.Infrastructures.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DNP.ThisIsAMainService.Shared.Infrastructures.Persistence;

internal static class PersistenceRegistration
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder, string nameOfConnectionString = "Default")
    {
        var connectionString = builder.Configuration.GetConnectionString(nameOfConnectionString);

        builder.Services.AddDbContextPool<DbContext, PeopleDbContext>(db =>
        {
            db.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure();
                options.MigrationsAssembly(typeof(PeopleDbContext).Assembly.GetName().Name);
            });

            db.UseSeeding((dbContext, _) =>
            {
                var peopleDbContext = (PeopleDbContext)dbContext;
                peopleDbContext.SeedingData();
            });

            db.UseAsyncSeeding(async (dbContext, _, cancellationToken) =>
            {
                await Task.Yield();
                var peopleDbContext = (PeopleDbContext)dbContext;
                peopleDbContext.SeedingData();
            });
        });

        builder.Services.AddScoped<IPersonRepository, PersonRepository>();

        return builder;
    }
}
