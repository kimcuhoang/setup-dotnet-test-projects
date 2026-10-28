using DNP.ThisIsAMainServices.TestsV1.Setup.TestContainerBuilders;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;

namespace DNP.ThisIsAMainServices.TestsV1.Setup;

public class TestAssemblyFixture : IAsyncLifetime
{
    private readonly string UniqueName = Guid.CreateVersion7().ToString("N");
    private readonly CancellationToken cancellationToken = TestContext.Current.CancellationToken;
    private INetwork _network = default!;
    private List<ITestContainerBuilder> _containers = [];
    public IEnumerable<ITestContainerBuilder> Containers => this._containers;

    private void GetNetwork()
    {
        this._network = new NetworkBuilder()
            .WithName($"BTL-Test-{this.UniqueName}")
            .WithCleanUp(true)
            .WithReuse(false)
            .Build();
    }

    private void GetTestContainers()
    {
        this._containers =
        [
            new MsSqlServerContainerBuilder(this._network, this.UniqueName, "ConnectionStrings:DbConnection")
        ];
    }

    public async ValueTask InitializeAsync()
    {
        this.GetNetwork();
        this.GetTestContainers();

        await this._network.CreateAsync(cancellationToken);
        await Task.WhenAll(this._containers.Select(container => container.StartAsync(this.cancellationToken)));
    }

    public async ValueTask DisposeAsync()
    {
        await Task.WhenAll(this._containers.Select(container => container.StopAsync(this.cancellationToken)));
        await this._network.DeleteAsync(cancellationToken);
        GC.SuppressFinalize(this);
    }
}