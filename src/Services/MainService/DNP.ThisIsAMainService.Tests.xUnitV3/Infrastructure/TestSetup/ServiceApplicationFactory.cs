using DNP.PeopleService.Tests.xUnitV3.Infrastructure.HostedServices;
using DNP.ThisIsAMainService.Infrastructures.Kestrel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace DNP.PeopleService.Tests.xUnitV3.Infrastructure.TestSetup;

public class ServiceApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString = default!;

    public ServiceApplicationFactory(string connectionString)
    {
        this._connectionString = connectionString;
        Debug.WriteLine($"{nameof(ServiceApplicationFactory)} constructor");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseSetting("Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information")
            .UseSetting("Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Warning");

        var settings = new List<KeyValuePair<string, string?>>
        {
            new("ConnectionStrings:Default", this._connectionString),
            new($"{nameof(AdvancedKestrelOptions)}:{nameof(AdvancedKestrelOptions.MinRequestBodyBytesPerSecond)}", "100"),
            new($"{nameof(AdvancedKestrelOptions)}:{nameof(AdvancedKestrelOptions.MinRequestBodyGracePeriodSeconds)}", "1"),
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        builder
            .UseEnvironment("Integration-Tests")
            .UseConfiguration(configuration)
            .ConfigureAppConfiguration(app => app.AddInMemoryCollection(settings));

        builder
            .ConfigureServices((context, services) =>
            {
                services.RemoveAll<IHostedService>();
            })
            .ConfigureTestServices(services =>
            {
                services.AddHostedService<StartupTestRunner>();
            });
    }

    public async Task ExecuteServiceAsync(Func<IServiceProvider, Task> func)
    {
        using var scope = this.Services.CreateAsyncScope();
        await func.Invoke(scope.ServiceProvider);
    }
}
