using DNP.ThisIsAMainServices.Tests.Setup.PreTest;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DNP.ThisIsAMainServices.Tests.Setup;

public class WebApplicationFactory(
    Action<IServiceCollection>? configureServices = null,
    Action<IConfigurationBuilder>? configureConfiguration = null) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder
            .UseEnvironment("Test-Integration")
            .UseSetting("Logging:LogLevel:Default", nameof(LogLevel.Information));

        var configurationBuilder = new ConfigurationBuilder();
        configureConfiguration?.Invoke(configurationBuilder);

        builder
            .UseConfiguration(configurationBuilder.Build())
            .ConfigureAppConfiguration(builder =>
            {
                configureConfiguration?.Invoke(builder);
            });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IHostedService>();
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddHostedService<DatabaseUp>();
            configureServices?.Invoke(services);
        });
    }
}