using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AzFunc.Example.Tests.Attempt02;

/// <summary>
/// https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=hostbuilder%2Cwindows#aspnet-core-integration
/// https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=hostbuilder%2Cwindows#version-2x
/// </summary>
public class AzureFunctionApplicationStartup
{
    public IHost AzFuncHost { get; private set; }
    public IHostBuilder AzFuncHostBuilder { get; }

    public AzureFunctionApplicationStartup()
    {
        AzFuncHostBuilder = new HostBuilder()
                            .ConfigureFunctionsWebApplication()
                            .ConfigureAppConfiguration((context, config) =>
                            {
                                config.AddInMemoryCollection(new Dictionary<string, string?>
                                {
                                    // { "AzureWebJobsStorage", "UseDevelopmentStorage=true" },
                                    // { "FUNCTIONS_WORKER_RUNTIME", "dotnet" },
                                    // { "AzureFunctionsJobHost__Logging__Console__IsEnabled", "true" },
                                    {"Functions:Worker:HostEndpoint", "http://localhost:7072" }
                                });
                            })
                            // .ConfigureWebJobs(new AzureFunctionStartup().Configure)
                            ;

        AzFuncHost = AzFuncHostBuilder.Build();
    }

    public async Task StartAsync()
    {
        await AzFuncHost.StartAsync();
    }

    public async Task StopAsync()
    {
        await AzFuncHost.StopAsync();
        AzFuncHost.Dispose();
    }
}


public class AzureFunctionStartup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
    }
}