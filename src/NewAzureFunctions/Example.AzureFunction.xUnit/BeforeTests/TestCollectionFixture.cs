using Example.AzureFunction.xUnit.DebuggerTools;

namespace Example.AzureFunction.xUnit.BeforeTests;

public class TestCollectionFixture: IAsyncLifetime
{
    public HttpClient Client { get; }
    public WorkingSession WorkingSession { get; }

    public TestCollectionFixture()
    {
        this.WorkingSession = new WorkingSession();
        this.Client = this.WorkingSession.Client;
    }

    public async Task InitializeAsync()
    {
        await this.WorkingSession.InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        await this.WorkingSession.DisposeAsync();
    }
}
