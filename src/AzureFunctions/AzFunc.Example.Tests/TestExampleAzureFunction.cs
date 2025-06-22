using AzFunc.Example.Tests.BeforeTests;
using AzFunc.Example.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace AzFunc.Example.Tests;
public class TestExampleAzureFunction(TestCollectionFixture testCollectionFixture): AzureFunctionTestBase(testCollectionFixture)
{
    private ExampleFunction ExampleFunction => this.Services.GetRequiredService<ExampleFunction>();
    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        this.ExampleFunction.ShouldNotBeNull();
    }

    private void VerifyOkResult(IActionResult? result)
        => result.ShouldSatisfyAllConditions(result =>
                {
                    result.ShouldNotBeNull();
                    result.ShouldBeOfType<OkObjectResult>();
                    var okResult = (OkObjectResult)result;
                    okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);
                    okResult.Value.ShouldBe(ExampleFunction.EXAMPLE_AZURE_FUNCTION_RESPONSE_TEXT);
                });

    [Fact(DisplayName = $"Playing with {nameof(HttpRequestData)}")]
    public async Task PlayingWithHttpRequestData()
    {
        var httpRequestData = new TestHttpRequestData("/api/http-request-data", HttpMethods.Get);
        // Act
        var result = await this.ExampleFunction.RunExampleFunction(httpRequestData);
        // Assert
        this.VerifyOkResult(result);
    }

    [Fact(DisplayName = $"Playing with {nameof(HttpRequest)}")]
    public async Task PlayingWithHttpRequest()
    {
        var httpRequest = new DefaultHttpContext().Request;
        httpRequest.Method = HttpMethods.Get;
        httpRequest.Path = "/api/http-request";

        // Act
        var result = await this.ExampleFunction.RunExampleFunctionWithHttpRequest(httpRequest);
        // Assert
        this.VerifyOkResult(result);
    }
}
