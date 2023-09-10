﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Xunit;

namespace DNP.PeopleService.Tests;

public class PeopleServiceWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _connectionString = default!;

    public PeopleServiceWebApplicationFactory(string connectionString)
    {
        this._connectionString = connectionString;
        Debug.WriteLine($"{nameof(PeopleServiceWebApplicationFactory)} constructor");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var settingsInMemory = new Dictionary<string, string?>
        {
            ["ConnectionStrings:Default"] = this._connectionString
        };

        var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settingsInMemory)
                .Build();

        builder
            .UseEnvironment("Integration-Test")
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseConfiguration(configuration)
            .UseTestServer()
            .ConfigureAppConfiguration(cfg =>
            {
                cfg.AddInMemoryCollection(settingsInMemory);
            })
            .ConfigureServices(services =>
            {
                services.RemoveAll<IHostedService>();
            })
            .ConfigureTestServices(services =>
            {
                //TODO: override services for testing only
            });
    }

    public async Task ExecuteServiceAsync(Func<IServiceProvider, Task> func)
    {
        using var scope = this.Services.CreateAsyncScope();
        await func.Invoke(scope.ServiceProvider);
    }

    public virtual async Task InitializeAsync()
    {
        await Task.Yield();

        Debug.WriteLine($"{nameof(PeopleServiceWebApplicationFactory)} {nameof(InitializeAsync)}");
    }

    public new virtual async Task DisposeAsync()
    {
        await Task.Yield();

        Debug.WriteLine($"{nameof(PeopleServiceWebApplicationFactory)} {nameof(DisposeAsync)}");
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
