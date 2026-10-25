using DNP.MainService.TestEdgeCases.SetupTestServer.Endpoints;
using DNP.MainService.TestEdgeCases.SetupTestServer.Kestrel;
using DNP.MainService.TestEdgeCases.SetupTestServer.Logging;
using DNP.MainService.TestEdgeCases.TestCases.SlowNetwork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace DNP.MainService.TestEdgeCases.SetupTestServer;

public class EdgeCaseWebApplicationFactory: IAsyncLifetime
{
    public static readonly double MinBytesPerSecond = 100;
    public static readonly TimeSpan GracePeriod = TimeSpan.FromSeconds(2);
    public WebApplication WebApplication { get; private set; }
    private readonly Channel<string> _kestrelEvents = Channel.CreateUnbounded<string>();
    public ChannelReader<string> KestrelEvents => this._kestrelEvents.Reader;

    private readonly List<KeyValuePair<string, string?>> Settings =
    [
        //new("Logging:LogLevel:Microsoft.AspNetCore.Server.Kestrel", "Warning"),
        new("Logging:LogLevel:Default", nameof(LogLevel.Error)),
        new($"{nameof(AdvancedKestrelOptions)}:{nameof(AdvancedKestrelOptions.MinRequestBodyBytesPerSecond)}", $"{MinBytesPerSecond}"),
        new($"{nameof(AdvancedKestrelOptions)}:{nameof(AdvancedKestrelOptions.MinRequestBodyGracePeriodSeconds)}", $"{GracePeriod.Seconds}")
    ];

    public async ValueTask InitializeAsync()
    {
        await Task.Yield();
        var webApplicationBuilder = WebApplication.CreateBuilder([]);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(this.Settings)
            .Build();

        webApplicationBuilder.WebHost.UseConfiguration(configuration);
        webApplicationBuilder
            .ConfigureKestrel(port: 12345)
            .AddCustomLogging();

        webApplicationBuilder.Services
                .AddSingleton(this._kestrelEvents)
                .AddHttpLogging()
                .AddHttpContextAccessor()
                .AddProblemDetails();


        this.WebApplication = webApplicationBuilder.Build();

        this.WebApplication
            .UseExceptionHandler()
            .UseHttpLogging()
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.AddSlowNetwork();
            });

        await this.WebApplication.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await this.WebApplication.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public HttpClient GetHttpClient()
    {
        var addressFeature = this.WebApplication.Services
                                            .GetRequiredService<IServer>()
                                            .Features.GetRequiredFeature<IServerAddressesFeature>();

        var serverAddress = addressFeature.Addresses.First();

        return new HttpClient
        {
            BaseAddress = new Uri(serverAddress),
            Timeout = GracePeriod + TimeSpan.FromSeconds(5)
        };
    }

    public HttpClient GetHttpClient(SlowNetworkHandler slowNetwork)
    {
        var addressFeature = this.WebApplication.Services
                                            .GetRequiredService<IServer>()
                                            .Features.GetRequiredFeature<IServerAddressesFeature>();

        var serverAddress = addressFeature.Addresses.First();

        return new HttpClient(slowNetwork)
        { 
            BaseAddress = new Uri(serverAddress),
            Timeout = GracePeriod + TimeSpan.FromSeconds(5)
        };
    }
}
