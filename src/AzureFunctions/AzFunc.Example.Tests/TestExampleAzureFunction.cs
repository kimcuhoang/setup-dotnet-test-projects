using AzFunc.Example.Tests.BeforeTests;
using AzFunc.Example.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;

namespace AzFunc.Example.Tests;
public class TestExampleAzureFunction(TestCollectionFixture testCollectionFixture) : AzureFunctionTestBase(testCollectionFixture)
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

    [Fact(DisplayName = $"Playing with WebApplicationFactory")]
    public async Task PlayingWithWebApplicationFactory()
    {
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder
                    .UseSetting("AzureFunctionsJobHost__Logging__Console__IsEnabled", "true")
                    .UseSetting("Functions:Worker:HostEndpoint", "http://localhost:5000");

                builder
                    .ConfigureKestrel(kestrel =>
                    {
                        kestrel.ListenLocalhost(5000, listenOptions =>
                        {
                             listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
                        });
                    });

                builder.CaptureStartupErrors(true);
                builder.UseTestServer();
            });



        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/http-request-data", CancellationToken.None);
        response.ShouldNotBeNull();
    }
}
