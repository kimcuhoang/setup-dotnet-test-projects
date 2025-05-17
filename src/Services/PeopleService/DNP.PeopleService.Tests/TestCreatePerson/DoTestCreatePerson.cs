using Bogus;
using Person = DNP.PeopleService.Domain.Person;
using Microsoft.EntityFrameworkCore;

namespace DNP.PeopleService.Tests.TestCreatePerson;
public class DoTestCreatePerson(PersonalServiceTestCollectionFixture testCollectionFixture, ITestOutputHelper testOutput) 
    : PeopleServiceTestBase(testCollectionFixture, testOutput)
{
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();

        await this.ExecuteTransactionDbContextAsync(async dbContext =>
        {
            await dbContext.Set<Person>().ExecuteDeleteAsync();
        });
    }

    [Fact]
    public async Task CreatePersonSuccessfully()
    {
        var person = new Faker<Person>()
                .RuleFor(_ => _.Id, _ => _.Random.Guid())
                .RuleFor(_ => _.Name, _ => _.Person.FullName)
                .Generate();

        await this.ExecuteTransactionDbContextAsync(async dbContext =>
        {
            dbContext.Add(person);
            await dbContext.SaveChangesAsync();
        });

        await this.ExecuteDbContextAsync(async dbContext =>
        {
            person = await dbContext.Set<Person>().FirstOrDefaultAsync(p => p.Id == person.Id);
            person.ShouldNotBeNull();
        });
    }
}
