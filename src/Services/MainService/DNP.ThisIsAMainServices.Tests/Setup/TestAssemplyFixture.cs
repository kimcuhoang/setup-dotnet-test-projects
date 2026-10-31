using DNP.ThisIsAMainServices.Tests.Setup.TestContainerBuilders;

namespace DNP.ThisIsAMainServices.Tests.Setup;

public class TestAssemblyFixture : IAsyncLifetime
{
    private readonly string UniqueName = Guid.CreateVersion7().ToString("N");
    private readonly CancellationToken cancellationToken = TestContext.Current.CancellationToken;
    private List<ITestContainerBuilder> _containers = [];
    public IEnumerable<ITestContainerBuilder> Containers => this._containers;


    public async ValueTask InitializeAsync()
    {
        this._containers =
        [
            new MsSqlServerContainerBuilder(this.UniqueName, "ConnectionStrings:Default")
        ];
        
        await Task.WhenAll(this._containers.Select(container => container.StartAsync(this.cancellationToken)));
    }

    public async ValueTask DisposeAsync()
    {
        await Task.WhenAll(this._containers.Select(container => container.StopAsync(this.cancellationToken)));
        GC.SuppressFinalize(this);
    }
}