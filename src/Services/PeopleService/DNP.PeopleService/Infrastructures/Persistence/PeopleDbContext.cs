using DNP.PeopleService.Features.People.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DNP.PeopleService.Infrastructures.Persistence;

public class PeopleDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Person> People => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public void SeedingData()
    {
        if (!People.Any(p => p.Id == Person.Default.Id))
        {
            People.Add(Person.Default);
            SaveChanges();
        }
    }
}
