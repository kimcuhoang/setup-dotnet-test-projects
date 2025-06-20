namespace AzFunc.Example.Tests.Attempt02;

[CollectionDefinition(nameof(TestAzureFunctionCollection))]
public class TestAzureFunctionCollection : ICollectionFixture<TestAzureFunctionCollectionFixture>
{}

public class TestAzureFunctionCollectionFixture : IAsyncLifetime
{
    public AzureFunctionApplicationStartup Startup { get; private set; }

    public TestAzureFunctionCollectionFixture()
    {
        Startup = new AzureFunctionApplicationStartup();
    }

    public async ValueTask InitializeAsync()
    {
        await Startup.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Startup.StopAsync();
    }
}