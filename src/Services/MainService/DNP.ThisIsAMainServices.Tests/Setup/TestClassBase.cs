namespace DNP.ThisIsAMainServices.Tests.Setup;

public abstract class TestClassBase(TestCollectionFixture collectionFixture, ITestOutputHelper testOutputHelper) : IAsyncLifetime
{
    protected WebApplicationFactory WebApplicationFactory { get;} = collectionFixture.WebApplicationFactory;
    protected ITestOutputHelper TestOutputHelper { get; } = testOutputHelper;
    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;
    protected readonly Faker Faker = new();
    protected AsyncServiceScope TestScope { get; private set; } = default!;
    protected IServiceProvider ScopeServiceProvider { get; private set; } = default!;

    public virtual async ValueTask InitializeAsync()
    {
        this.TestScope = this.WebApplicationFactory.Services.CreateAsyncScope();
        this.ScopeServiceProvider = this.TestScope.ServiceProvider;
        await ValueTask.CompletedTask;
    }

    public virtual async ValueTask DisposeAsync()
    {
        await this.TestScope.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}