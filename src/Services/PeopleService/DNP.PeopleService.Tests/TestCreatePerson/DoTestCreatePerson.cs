using Bogus;
using Person = DNP.PeopleService.Domain.Person;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace DNP.PeopleService.Tests.TestCreatePerson;
public class DoTestCreatePerson(PersonalServiceTestCollectionFixture testCollectionFixture, ITestOutputHelper testOutput) 
    : PeopleServiceTestBase(testCollectionFixture, testOutput)
{
    protected readonly List<Guid> _personIds = new();

    private void DeletePerson(Guid? personId = null)
    {
        if (personId == null) return;
        this._personIds.Add(personId.Value);
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();

        await this.ExecuteTransactionDbContextAsync(async dbContext =>
        {
            await dbContext.Set<Person>()
                    .Where(_ => this._personIds.Contains(_.Id))
                    .ExecuteDeleteAsync();
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

            this.DeletePerson(person.Id);
        });

        await this.ExecuteDbContextAsync(async dbContext =>
        {
            person = await dbContext.Set<Person>().FirstOrDefaultAsync(p => p.Id == person.Id);
            person.Should().NotBeNull();
        });
    }
}
