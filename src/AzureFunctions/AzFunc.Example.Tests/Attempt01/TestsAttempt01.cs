using System.Net;
using Shouldly;

namespace AzFunc.Example.Tests.Attempt01;

[Collection(nameof(TestCollection))]
public abstract class IntegrationTestBase(TestCollectionFixture testCollectionFixture) : IAsyncLifetime
{
    public virtual ValueTask InitializeAsync() => ValueTask.CompletedTask;
    public virtual ValueTask DisposeAsync() => ValueTask.CompletedTask;
}


public class TestHttpAzureFunction(TestCollectionFixture testCollectionFixture) : IntegrationTestBase(testCollectionFixture)
{
    [Fact]
    public async Task Test1()
    {
        var client = testCollectionFixture.ApplicationFactory.CreateDefaultClient();
        var response = await client.GetAsync("/api/ExampleFunction", CancellationToken.None);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}