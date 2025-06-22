using AzFunc.Example.Tests.BeforeTests;
using AzFunc.Example.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace AzFunc.Example.Tests;
public class TestExampleAzureFunction(TestCollectionFixture testCollectionFixture) : AzureFunctionTestBase(testCollectionFixture)
{
    private ExampleFunction ExampleFunction => this.Services.GetRequiredService<ExampleFunction>();
    private TestFunctionContext TestFunctionContext => new("ExampleFunction")
    {
        InstanceServices = this.Services
    };

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        this.ExampleFunction.ShouldNotBeNull();

    }

    [Fact(DisplayName = "ExampleFunction should be able to execute successfully")]
    public async Task ExampleFunction_Should_Execute_Successfully()
    {
        var httpRequestData = new TestHttpRequestData(HttpMethods.Get, this.TestFunctionContext);
        // Act
        var result = await this.ExampleFunction.RunExampleFunction(httpRequestData);
        // Assert
        result.ShouldSatisfyAllConditions(result =>
        {
            result.ShouldNotBeNull();
            result.ShouldBeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);
            okResult.Value.ShouldNotBeNull();
        });
    }
}
