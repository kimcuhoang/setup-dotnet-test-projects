using Microsoft.EntityFrameworkCore;

namespace DNP.PeopleService.Persistence;

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

        return builder;
    }
}
