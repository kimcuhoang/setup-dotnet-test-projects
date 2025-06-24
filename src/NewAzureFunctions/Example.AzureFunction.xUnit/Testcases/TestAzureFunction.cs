using Example.AzureFunction.xUnit.BeforeTests;
using Shouldly;

namespace Example.AzureFunction.xUnit.Testcases;

public class TestAzureFunction(TestCollectionFixture testCollectionFixture) : AzureFunctionTestBase(testCollectionFixture)
{
    [Fact]
    public async Task TestFunction()
    {
        var response = await this.Client.GetAsync("/api/SimpleHttp");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("Welcome to Azure Functions!");
    }

    [Fact]
    public async Task TestNotFoundFunction()
    {
        var response = await this.Client.GetAsync("/api/ShouldBeNotFound");
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
    }
}
