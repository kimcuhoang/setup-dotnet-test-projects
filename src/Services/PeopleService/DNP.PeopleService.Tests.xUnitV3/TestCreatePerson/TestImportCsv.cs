using NPOI.XSSF.UserModel;
using System.Text;
using static DNP.PeopleService.Features.People.ImportController;

namespace DNP.PeopleService.Tests.xUnitV3.TestCreatePerson;
public class TestImportCsv(ServiceTestAssemblyFixture testCollectionFixture, ITestOutputHelper testOutputHelper)
    : ServiceTestBase(testCollectionFixture, testOutputHelper)
{
    [Fact]
    public async Task ImportPersonCodeViaCsv()
    {
        await this.ExecuteHttpClientAsync(async httpClient =>
        {
            var sb = new StringBuilder().AppendLine("Code");

            var numberOfCodes = 10;

            for (int i = 0; i < numberOfCodes; i++)
            {
                sb.AppendLine(Faker.Random.AlphaNumeric(10).ToUpper());
            }


            var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var csvStream = new MemoryStream(csvBytes);
            using var formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(csvStream), name: "file", fileName: "abc.csv");

            var response = await httpClient.PostAsync("/import-csv", formData);
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var codes = await ParseResponse<List<PersonCode>>(response);
            codes.ShouldNotBeEmpty();
            codes.Count.ShouldBe(numberOfCodes);

            codes!.ForEach(_ =>
            {
                _.ShouldNotBeNull();
                _.Code.ShouldNotBeNullOrEmpty();
            });
        });
    }

    [Fact]
    public async Task ImportPersonCodeViaExcel()
    {
        await this.ExecuteHttpClientAsync(async httpClient =>
        {
            const int numberOfCodes = 10;

            var workBook = new XSSFWorkbook();
            var sheet = workBook.CreateSheet("Sheet1");
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Code");

            for (int i = 1; i <= numberOfCodes; i++)
            {
                var dataRow = sheet.CreateRow(i);
                dataRow.CreateCell(0).SetCellValue(Faker.Random.AlphaNumeric(10).ToUpper());
            }

            var excelStream = new MemoryStream();
            workBook.Write(excelStream, leaveOpen: true);

            using var formData = new MultipartFormDataContent();
            formData.Add(content: new StreamContent(excelStream),
                        name: "file",
                        fileName: "abc.excel");

            var response = await httpClient.PostAsync("/import-excel", formData);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var codes = await ParseResponse<List<PersonCode>>(response);
            codes.ShouldNotBeEmpty();
            codes.Count.ShouldBe(numberOfCodes);

            codes!.ForEach(_ =>
            {
                _.ShouldNotBeNull();
                _.Code.ShouldNotBeNullOrEmpty();
            });
        });
    }
}
