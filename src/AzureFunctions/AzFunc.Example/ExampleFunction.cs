using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzFunc.Example;

public class ExampleFunction(ILogger<ExampleFunction> logger)
{
    public const string EXAMPLE_AZURE_FUNCTION_RESPONSE_TEXT = "Welcome to Azure Functions!";

    [Function("ExampleFunction")]
    public async Task<IActionResult> RunExampleFunction(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "http-request-data")] HttpRequestData req)
    {
        await Task.Yield();
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult(EXAMPLE_AZURE_FUNCTION_RESPONSE_TEXT);
    }

    [Function("ExampleFunctionHttpRequest")]
    public async Task<IActionResult> RunExampleFunctionWithHttpRequest(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "http-request" )] HttpRequest req)
    {
        await Task.Yield();
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult(EXAMPLE_AZURE_FUNCTION_RESPONSE_TEXT);
    }
}
