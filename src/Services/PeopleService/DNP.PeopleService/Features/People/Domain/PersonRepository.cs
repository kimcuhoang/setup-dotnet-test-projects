using DNP.PeopleService.Infrastructures.Persistence;

namespace DNP.PeopleService.Features.People.Domain;

public interface IPersonRepository
{
    Task Add(Person person, CancellationToken cancellationToken = default);
}

public class PersonRepository : IPersonRepository
{
    private readonly PeopleDbContext _dbContext;
    public PersonRepository(PeopleDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Person person, CancellationToken cancellationToken = default)
    {
        var existingPerson = await this._dbContext.People.FindAsync([person.Id], cancellationToken);
        if (existingPerson is not null)
        {
            throw new InvalidOperationException($"Person with ID {person.Id} already exists.");
        }
        this._dbContext.People.Add(person);
        await this._dbContext.SaveChangesAsync(cancellationToken);
    }
}
