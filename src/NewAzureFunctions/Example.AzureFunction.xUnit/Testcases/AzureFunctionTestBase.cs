using Example.AzureFunction.xUnit.BeforeTests;

namespace Example.AzureFunction.xUnit.Testcases;

[Collection(nameof(TestCollection))]
public abstract class AzureFunctionTestBase(TestCollectionFixture testCollectionFixture) : IAsyncLifetime
{
    protected HttpClient Client => testCollectionFixture.Client;
    public virtual async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        await Task.CompletedTask;
    }
}
