using FluentAssertions;
using NPOI.XSSF.UserModel;
using System.Net;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static DNP.PeopleService.Controllers.ImportController;

namespace DNP.PeopleService.Tests.TestCreatePerson;
public class TestImportCsv(PersonalServiceTestCollectionFixture testCollectionFixture, ITestOutputHelper testOutput) : PeopleServiceTestBase(testCollectionFixture, testOutput)
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
                sb.AppendLine(this._faker.Random.AlphaNumeric(10).ToUpper());
            }


            var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var csvStream = new MemoryStream(csvBytes);
            using var formData = new MultipartFormDataContent();
            formData.Add(new StreamContent(csvStream), name: "file", fileName: "abc.csv");

            var response = await httpClient.PostAsync("/import-csv", formData);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var codes = await this.ParseResponse<List<PersonCode>>(response);
            codes.Should().HaveCount(numberOfCodes);
            codes!.ForEach(_ =>
            {
                _.Should().NotBeNull();
                _.Code.Should().NotBeNullOrWhiteSpace();
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
                dataRow.CreateCell(0).SetCellValue(this._faker.Random.AlphaNumeric(10).ToUpper());
            }

            var excelStream = new MemoryStream();
            workBook.Write(excelStream, leaveOpen: true);

            using var formData = new MultipartFormDataContent();
            formData.Add(content: new StreamContent(excelStream), 
                        name: "file", 
                        fileName: "abc.excel");

            var response = await httpClient.PostAsync("/import-excel", formData);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var codes = await this.ParseResponse<List<PersonCode>>(response);
            codes.Should().HaveCount(numberOfCodes);
            codes!.ForEach(_ =>
            {
                _.Should().NotBeNull();
                _.Code.Should().NotBeNullOrWhiteSpace();
            });
        });
    }
}
