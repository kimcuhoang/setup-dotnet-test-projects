using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Example.AzureFunction;

public class SimpleHttpAzureFunction(ILogger<SimpleHttpAzureFunction> logger)
{
    [Function("SimpleHttp")]
    public Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return Task.FromResult<IActionResult>(new OkObjectResult("Welcome to Azure Functions!"));
    }
}