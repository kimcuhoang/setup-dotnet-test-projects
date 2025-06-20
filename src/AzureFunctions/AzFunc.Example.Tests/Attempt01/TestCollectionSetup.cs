
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace AzFunc.Example.Tests.Attempt01;

[CollectionDefinition(nameof(TestCollection))]
public class TestCollection : ICollectionFixture<TestCollectionFixture>
{

}

public class AzureFunctionApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseTestServer()
            .UseSetting("Functions:Worker:HostEndpoint", "http://localhost:7072");
    }

    protected override void ConfigureClient(HttpClient client)
    {
        client.BaseAddress = new Uri("http://localhost:7072");
    }
}

public class TestCollectionFixture : IAsyncLifetime
{
    public AzureFunctionApplicationFactory ApplicationFactory { get; private set; }
    public TestCollectionFixture()
    {
        // This constructor can be used to initialize any resources needed before tests run.
        // For example, you could start an Azure Function host or set up a test environment.
        Debug.WriteLine($"{nameof(TestCollectionFixture)} {nameof(TestCollectionFixture)}");
        ApplicationFactory = new AzureFunctionApplicationFactory();
    }

    public async ValueTask InitializeAsync()
    {
        Debug.WriteLine($"{nameof(TestCollectionFixture)} {nameof(InitializeAsync)}");
        await Task.Yield();
    }

    public async ValueTask DisposeAsync()
    {
        Debug.WriteLine($"{nameof(TestCollectionFixture)} {nameof(DisposeAsync)}");
        GC.SuppressFinalize(this);
        await Task.Yield();
    }
}