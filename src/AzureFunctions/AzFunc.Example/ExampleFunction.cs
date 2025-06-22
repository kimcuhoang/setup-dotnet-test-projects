using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFunc.Example;

public class ExampleFunction(ILogger<ExampleFunction> logger)
{
    [Function("ExampleFunction")]
    public async Task<IActionResult> RunExampleFunction([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        await Task.Yield();
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}
