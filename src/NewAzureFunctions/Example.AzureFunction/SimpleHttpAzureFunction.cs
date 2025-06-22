using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Example.AzureFunction;

public class SimpleHttpAzureFunction
{
    private readonly ILogger<SimpleHttpAzureFunction> _logger;

    public SimpleHttpAzureFunction(ILogger<SimpleHttpAzureFunction> logger)
    {
        _logger = logger;
    }

    [Function("SimpleHttp")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}