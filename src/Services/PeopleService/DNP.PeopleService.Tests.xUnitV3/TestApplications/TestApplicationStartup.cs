namespace DNP.PeopleService.Tests.xUnitV3.TestApplications;
public class TestApplicationStartup(ServiceTestAssemblyFixture testCollectionFixture, ITestOutputHelper testOutputHelper) : ServiceTestBase(testCollectionFixture, testOutputHelper)
{
    [Fact]
    public async Task EnsureDefaultPersonHasBeenSeeded()
    {
        await this.ExecuteDbContextAsync(async dbContext =>
        {
            var person = await dbContext.Set<PersonDomain>()
                        .FirstOrDefaultAsync(c => c.Id == PersonDomain.Default.Id, this.CancellationToken);
            person.ShouldNotBeNull("because the default person should be seeded in the database");
        });
    }
}
