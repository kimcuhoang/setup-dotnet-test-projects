using DNP.ThisIsAMainService.Tests.Setup.TestContainerBuilders;

namespace DNP.ThisIsAMainService.Tests.Setup;

public class TestCollectionFixture(TestAssemblyFixture assemblyFixture) : IAsyncLifetime
{
    public WebApplicationFactory WebApplicationFactory { get; private set; } = default!;
    private readonly IEnumerable<ITestContainerBuilder> testContainers = assemblyFixture.Containers;
    private readonly ITestOutputHelper? testOutputHelper = TestContext.Current.TestOutputHelper;

    public async ValueTask InitializeAsync()
    {
        this.WebApplicationFactory = new WebApplicationFactory(
            configureConfiguration: configurationBuilder =>
            {
                foreach (var container in this.testContainers.OfType<IHasConnectionString>())
                {
                    container.ConfigureConnectionString(configurationBuilder);
                    this.testOutputHelper?.WriteLine(container.ConnectionString);
                }
            }
        ); 
    }

    public async ValueTask DisposeAsync()
    {
        await this.WebApplicationFactory.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}

