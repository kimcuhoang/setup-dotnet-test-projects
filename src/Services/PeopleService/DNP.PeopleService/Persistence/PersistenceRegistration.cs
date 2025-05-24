//using DNP.PeopleService.Features.People.Domain;
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

            //db.UseSeeding((dbContext, _) =>
            //{
            //    var peopleDbContext = (PeopleDbContext)dbContext;
            //    var defaultPerson = peopleDbContext.People.FirstOrDefault(p => p.Id == Person.Default.Id);
            //    if (defaultPerson is null)
            //    {
            //        peopleDbContext.Add(Person.Default);
            //        peopleDbContext.SaveChanges();
            //    }
            //});

            //db.UseAsyncSeeding(async (dbContext, _, cancellationToken) =>
            //{
            //    var peopleDbContext = (PeopleDbContext)dbContext;
            //    var defaultPerson = await peopleDbContext.People.FirstOrDefaultAsync(p => p.Id == Person.Default.Id, cancellationToken);
            //    if (defaultPerson is null)
            //    {
            //        peopleDbContext.Add(Person.Default);
            //        await peopleDbContext.SaveChangesAsync(cancellationToken);
            //    }
            //});
        });

        return builder;
    }
}
