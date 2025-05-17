using Person = DNP.PeopleService.Domain.Person;


namespace DNP.PeopleService.Tests.xUnitV3.TestCreatePerson;
public class DoTestCreatePerson(ServiceTestAssemblyFixture testCollectionFixture, ITestOutputHelper testOutputHelper)
        : ServiceTestBase(testCollectionFixture, testOutputHelper)
{
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();

        await ExecuteTransactionDbContextAsync(async dbContext =>
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

        await ExecuteTransactionDbContextAsync(async dbContext =>
        {
            dbContext.Add(person);
            await dbContext.SaveChangesAsync();
        });

        await ExecuteDbContextAsync(async dbContext =>
        {
            person = await dbContext.Set<Person>().FirstOrDefaultAsync(p => p.Id == person.Id);
            person.ShouldNotBeNull();
        });
    }
}
