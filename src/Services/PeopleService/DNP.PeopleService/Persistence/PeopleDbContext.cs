using DNP.PeopleService.Features.People.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DNP.PeopleService.Persistence;

public class PeopleDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Person> People => this.Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public void SeedingData()
    {
        if (!this.People.Any(p => p.Id == Person.Default.Id))
        {
            this.People.Add(Person.Default);
            this.SaveChanges();
        }
    }
}
