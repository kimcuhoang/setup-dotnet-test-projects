using DNP.MainService.TestEdgeCases.SetupTestServer;
using DNP.MainService.TestEdgeCases.SetupTestServer.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using System.Net.Http.Json;
using System.Net.Sockets;

namespace DNP.MainService.TestEdgeCases.TestCases.SlowNetwork;

public class SlowNetworkTestCase(EdgeCaseWebApplicationFactory factory, ITestOutputHelper testOutputHelper) : IClassFixture<EdgeCaseWebApplicationFactory>, IAsyncLifetime
{
    private readonly WebApplication webApplication = factory.WebApplication;
    private readonly CancellationToken cancellationToken = TestContext.Current.CancellationToken;
    private IServiceProvider ScopeServiceProvider;
    private const string Api = "/integration-tests/slow-network-simulator";

    public async ValueTask InitializeAsync()
    {
        var scope = this.webApplication.Services.CreateAsyncScope();
        this.ScopeServiceProvider = scope.ServiceProvider;
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    [Fact]
    public void EnsureWeAreUsingKestrel()
    {
        var server = this.ScopeServiceProvider.GetRequiredService<IServer>();
        server.ShouldSatisfyAllConditions(s =>
        {
            s.GetType().FullName.ShouldSatisfyAllConditions(n =>
            {
                n.ShouldNotBeNullOrWhiteSpace();
                n.ShouldBe("Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerImpl");
            });
        });
    }

    [Fact]
    public async Task Upload_SlowNetwork_ShouldTrigger_MinRequestBodyDataRate()
    {
        // Arrange
        var bytesPerChunk = (int)(EdgeCaseWebApplicationFactory.MinBytesPerSecond * 0.5);

        var handler = new SlowNetworkHandler(bytesPerChunk);
        var client = factory.GetHttpClient(handler);

        var totalBytes = (int)(EdgeCaseWebApplicationFactory.GracePeriod.TotalSeconds + 1) * bytesPerChunk;
        var model = new PostDataModel(Message: new string('a', totalBytes));

        // Act
        var exception = await Should.ThrowAsync<HttpRequestException>(async () =>
        {
            await client.PostAsJsonAsync(Api, model, this.cancellationToken);
        });

        exception.ShouldNotBeNull()
            .InnerException.ShouldNotBeNull().ShouldBeOfType<IOException>()
            .InnerException.ShouldNotBeNull().ShouldBeOfType<SocketException>()
            .SocketErrorCode.ShouldBe(SocketError.ConnectionAborted);

        // Confirm on the server side via Kestrel logs
        await Task.Delay(500, this.cancellationToken);

        var logs = new List<string>();
        while (factory.KestrelEvents.TryRead(out var entry))
        {
            logs.Add(entry);
            testOutputHelper.WriteLine(entry);
        }

        logs.ShouldNotBeEmpty();
        logs.ShouldContain(log => log.Contains("MinRequestBodyDataRate", StringComparison.OrdinalIgnoreCase));
    }
}
