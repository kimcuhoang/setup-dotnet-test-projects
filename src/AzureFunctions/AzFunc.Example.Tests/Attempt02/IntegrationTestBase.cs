using System.Net;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Shouldly;

namespace AzFunc.Example.Tests.Attempt02;

[Collection(nameof(TestAzureFunctionCollection))]
public abstract class IntegrationTestBase(TestAzureFunctionCollectionFixture testCollectionFixture) : IAsyncLifetime
{
    protected IHost Host => testCollectionFixture.Startup.AzFuncHost;
    public virtual ValueTask InitializeAsync() => ValueTask.CompletedTask;
    public virtual ValueTask DisposeAsync() => ValueTask.CompletedTask;    
}


public class ThisIsATestClass(TestAzureFunctionCollectionFixture testCollectionFixture) : IntegrationTestBase(testCollectionFixture)
{
    [Fact]
    public async Task Test1()
    {
        var httpClient = Host.GetTestClient();
        var response = await httpClient.GetAsync("/api/ExampleFunction", CancellationToken.None);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}