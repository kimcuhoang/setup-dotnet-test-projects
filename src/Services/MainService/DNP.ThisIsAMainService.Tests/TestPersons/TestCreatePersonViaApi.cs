using System.Net.Http.Json;
using DNP.ThisIsAMainService.Features.People.Create;
using Microsoft.AspNetCore.Mvc;

namespace DNP.ThisIsAMainService.Tests.TestPersons;

public class TestCreatePersonViaApi(TestCollectionFixture collectionFixture, ITestOutputHelper testOutputHelper) : TestClassBase(collectionFixture, testOutputHelper)
{
    private static readonly string API_PATH = $"/people{CreatePeopleEndpoint.Route}";

    [Fact]
    public async Task CreatePersonSuccessfully()
    {
        var payload = new CreatePersonPayload("Success");
        var httpClient = this.WebApplicationFactory.CreateClient();

        var response = await httpClient.PostAsJsonAsync(API_PATH, payload, this.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task FailDueToNotAcceptable()
    {
        var payload = new CreatePersonPayload("DUPLICATE");
        var httpClient = this.WebApplicationFactory.CreateClient();

        var response = await httpClient.PostAsJsonAsync(API_PATH, payload, this.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.NotAcceptable);
    }

    [Fact]
    public async Task FailDueToExpectedException()
    {
        var payload = new CreatePersonPayload("EXCEPTION");
        var httpClient = this.WebApplicationFactory.CreateClient();

        var response = await httpClient.PostAsJsonAsync(API_PATH, payload, this.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        var problemDetail = await response.Content.ReadFromJsonAsync<ProblemDetails>(this.CancellationToken);
        problemDetail.ShouldNotBeNull();
    }
}