
using AzFunc.Example.Tests.BeforeTests;

namespace AzFunc.Example.Tests;
[Collection(nameof(TestCollection))]
public abstract class AzureFunctionTestBase(TestCollectionFixture testCollectionFixture) : IAsyncLifetime
{
    protected IServiceProvider Services { get; } = testCollectionFixture.Services;

    public virtual async ValueTask InitializeAsync()
    {
        await ValueTask.CompletedTask;
    }

    public virtual async ValueTask DisposeAsync()
    {
        await ValueTask.CompletedTask;
    }
}
