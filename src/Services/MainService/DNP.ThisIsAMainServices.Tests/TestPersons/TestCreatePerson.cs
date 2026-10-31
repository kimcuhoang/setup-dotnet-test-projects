namespace DNP.ThisIsAMainServices.Tests.TestPersons;

public class TestCreatePerson(TestCollectionFixture collectionFixture, ITestOutputHelper testOutputHelper) : TestClassBase(collectionFixture, testOutputHelper)
{
    public override async ValueTask DisposeAsync()
    {
        var dbContext = this.ScopeServiceProvider.GetRequiredService<DbContext>();
        await dbContext.Set<PersonDomain>()
                .Where(_ => _.Id != PersonDomain.Default.Id)
                .ExecuteDeleteAsync(this.CancellationToken);

        await base.DisposeAsync();
    }

    [Fact]
    public async Task TestPreSeeding()
    {
        var dbContext = this.ScopeServiceProvider.GetRequiredService<DbContext>();
        var person = await dbContext.Set<PersonDomain>().FirstOrDefaultAsync(this.CancellationToken);
        person.ShouldNotBeNull("because the default person should be seeded in the database");
    }

    [Fact]
    public async Task CreatePersonSuccessfully()
    {
        var person = new Faker<PersonDomain>()
                .RuleFor(_ => _.Id, _ => _.Random.Guid())
                .RuleFor(_ => _.Name, _ => _.Person.FullName)
                .Generate();

        var dbContext = this.ScopeServiceProvider.GetRequiredService<DbContext>();
        dbContext.Add(person);
        var savedResult = await dbContext.SaveChangesAsync(this.CancellationToken);
        savedResult.ShouldBeGreaterThan(0);

        var savedPerson = await dbContext.Set<PersonDomain>().FirstOrDefaultAsync(p => p.Id == person.Id, this.CancellationToken);
        savedPerson.ShouldNotBeNull();
    }
}