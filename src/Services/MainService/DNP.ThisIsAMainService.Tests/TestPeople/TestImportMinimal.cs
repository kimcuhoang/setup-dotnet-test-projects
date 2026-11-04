using System.Net.Http.Json;
using System.Text;

using DNP.ThisIsAMainService.Features.People.ImportMinimal;

namespace DNP.ThisIsAMainService.Tests.TestPeople;

public class TestImportMinimal(TestCollectionFixture collectionFixture, ITestOutputHelper testOutputHelper) : TestClassBase(collectionFixture, testOutputHelper)
{
    private static readonly string API_PATH = $"/people{ImportMinimalEndpoint.Route}";
    [Fact]
    public async Task ShouldBeSuccess()
    {
        var sb = new StringBuilder().AppendLine("Code");

        var numberOfCodes = 10;

        for (int i = 0; i < numberOfCodes; i++)
        {
            sb.AppendLine(this.Faker.Random.AlphaNumeric(10).ToUpper());
        }

        var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
        var csvStream = new MemoryStream(csvBytes);
        using var formData = new MultipartFormDataContent();
        formData.Add(new StringContent("NameOfIMport-ABC"), name: "NameOfIMport");
        formData.Add(new StreamContent(csvStream), name: "ImportFile", fileName: "abc.csv");

        var httpClient = this.WebApplicationFactory.CreateClient();
        var response = await httpClient.PostAsync(API_PATH, formData, this.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var results = await response.Content.ReadFromJsonAsync<List<ImportMinimalPersonModel>>(this.CancellationToken);
        results.ShouldNotBeNull().ShouldNotBeEmpty();
        results.Count.ShouldBe(numberOfCodes);
    }
}