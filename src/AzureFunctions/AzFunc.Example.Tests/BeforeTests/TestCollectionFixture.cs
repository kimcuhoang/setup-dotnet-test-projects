using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzFunc.Example.Tests.BeforeTests;
public class TestCollectionFixture : IAsyncLifetime
{
    public IServiceProvider Services { get; }

    private readonly IHost HostInstance;

    public TestCollectionFixture()
    {
        this.HostInstance = new HostBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<ExampleFunction>();
            })
            .Build();
        this.Services = this.HostInstance.Services;
    }

    public async ValueTask InitializeAsync()
    {
        await Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        this.HostInstance.Dispose();
        await Task.CompletedTask;
    }
}
