namespace DNP.PeopleService.Tests.xUnitV3.TestCreatePerson;

//public class DoTestCreatePerson(ServiceTestAssemblyFixture testCollectionFixture, ITestOutputHelper testOutputHelper)
//        : ServiceTestBase(testCollectionFixture, testOutputHelper)

public class DoTestCreatePerson(TestCollectionFixture testCollectionFixture, ITestOutputHelper testOutputHelper, ITestContextAccessor testContextAccessor)
                : IntegrationTestBase(testCollectionFixture, testOutputHelper, testContextAccessor)
{
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();

        await this.ExecuteTransactionDbContextAsync(async dbContext =>
        {
            await dbContext.Set<PersonDomain>()
                    .Where(_ => _.Id != PersonDomain.Default.Id)
                    .ExecuteDeleteAsync();
        });
    }

    [Fact]
    public async Task TestPreSeeding()
    {
        await this.ExecuteDbContextAsync(async dbContext =>
        {
            var person = await dbContext.Set<PersonDomain>().FirstOrDefaultAsync();
            person.ShouldNotBeNull();
        });
    }

    [Fact]
    public async Task CreatePersonSuccessfully()
    {
        var person = new Faker<PersonDomain>()
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
            person = await dbContext.Set<PersonDomain>().FirstOrDefaultAsync(p => p.Id == person.Id);
            person.ShouldNotBeNull();
        });
    }
}
