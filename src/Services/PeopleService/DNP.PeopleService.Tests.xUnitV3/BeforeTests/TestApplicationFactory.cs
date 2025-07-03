using DNP.PeopleService.Infrastructures.FileStorages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DNP.PeopleService.Tests.xUnitV3.BeforeTests;
public class TestApplicationFactory : WebApplicationFactory<Program>
{
    private readonly List<KeyValuePair<string, string?>> InMemorySettings =
    [
        new KeyValuePair<string, string?>("Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information"),
    ];

    public string ConnectionString
    {
        init => this.InMemorySettings.Add(new KeyValuePair<string, string?>("ConnectionStrings:Default", value));
    }

    public string AzureStorageConnectionString
    {
        init => this.InMemorySettings
            .Add(new KeyValuePair<string, string?>(
                $"{nameof(FileStorageOptions)}:{nameof(FileStorageOptions.AzureBlobOptions)}:{nameof(AzureBlobOptions.ConnectionString)}", 
                value));
    }

    public JsonSerializerOptions JsonSerializerSettings
    {
        get
        {
            var jsonSettings = this.Services.GetRequiredService<IOptions<JsonOptions>>().Value;
            return jsonSettings?.SerializerOptions ?? new JsonSerializerOptions();
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(this.InMemorySettings)
            .Build();

        builder
            .UseEnvironment("Integration-Test")
            .UseConfiguration(configuration)
            .UseSetting("Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Warning")
            .ConfigureServices(services =>
            {
                services.RemoveAll<IHostedService>();
            })
            .ConfigureTestServices(services =>
            {
                services.AddHostedService<StartupTestRunner>();
            });
    }
}
