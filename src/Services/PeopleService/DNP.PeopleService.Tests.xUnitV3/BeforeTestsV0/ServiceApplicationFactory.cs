using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace DNP.PeopleService.Tests.xUnitV3.BeforeTestsV0;

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
        builder.UseEnvironment("Integration-Test");

        builder
            .UseSetting("ConnectionStrings:Default", this._connectionString)
            .UseSetting("Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Information")
            .UseSetting("Logging:LogLevel:Microsoft.EntityFrameworkCore.Database.Command", "Warning");

        builder
            .ConfigureServices(services =>
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

    public JsonSerializerOptions JsonSerializerSettings
    {
        get
        {
            var jsonSettings = this.Services.GetRequiredService<IOptions<JsonOptions>>().Value;
            return jsonSettings?.SerializerOptions ?? new JsonSerializerOptions();
        }
    }
}
