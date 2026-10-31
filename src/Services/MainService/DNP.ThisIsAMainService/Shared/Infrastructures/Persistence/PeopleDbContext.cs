using DNP.ThisIsAMainService.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DNP.ThisIsAMainService.Shared.Infrastructures.Persistence;

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
