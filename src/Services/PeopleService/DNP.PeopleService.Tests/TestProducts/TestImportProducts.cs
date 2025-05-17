using DNP.PeopleService.Features.Products.Models;
using NPOI.XSSF.UserModel;
using System.Net;
using System.Text;

namespace DNP.PeopleService.Tests.TestProducts;
public class TestImportProducts(PersonalServiceTestCollectionFixture testCollectionFixture, ITestOutputHelper testOutput) : IntegrationTestBase(testCollectionFixture, testOutput)
{
    [Fact]
    public async Task TestImportByCsv()
    {
        // ==================================
        // Step-01: Generate the CSV stream
        // ==================================
        var sb = new StringBuilder().AppendLine("Product Code,Product Name");

        var numberOfProducts = 10;
        for (int i = 0; i < numberOfProducts; i++)
        {
            sb.AppendLine(string.Join(",", [
                this._faker.Random.AlphaNumeric(10).ToUpper(),
                    this._faker.Commerce.ProductName()
                ]));
        }

        var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
        var csvStream = new MemoryStream(csvBytes);

        // ==================================
        // Step-02: Prepare the FormData
        // ==================================
        using var formData = new MultipartFormDataContent();
        formData.Add(new StreamContent(csvStream), name: "file", fileName: "abc.csv");

        // ==================================
        // Step-03: Execute the POST request
        // ==================================
        var httpClient = this._factory.CreateClient();
        var response = await httpClient.PostAsync("/products/import-csv", formData, this.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var products = await this.ParseResponse<List<ImportProductModel>>(response);

        products.ShouldNotBeEmpty();
        products.Count.ShouldBe(numberOfProducts);
        products.ForEach(p =>
        {
            p.Code.ShouldNotBeNullOrWhiteSpace();
            p.Name.ShouldNotBeNullOrWhiteSpace();
        });
    }

    [Fact]
    public async Task TestImportViaExcel()
    {
        // ==================================
        // Step-01: Generate the Excel stream
        // ==================================
        var workBook = new XSSFWorkbook();
        var sheet = workBook.CreateSheet("Sheet1");
        var headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Product Code");
        headerRow.CreateCell(1).SetCellValue("Product Name");

        const int numberOfProducts = 10;
        for (int i = 1; i <= numberOfProducts; i++)
        {
            var dataRow = sheet.CreateRow(i);
            dataRow.CreateCell(0).SetCellValue(this._faker.Random.AlphaNumeric(10).ToUpper());
            dataRow.CreateCell(1).SetCellValue(this._faker.Commerce.ProductName());
        }

        var excelStream = new MemoryStream();
        workBook.Write(excelStream, leaveOpen: true);

        // ==================================
        // Step-02: Prepare the FormData
        // ==================================
        using var formData = new MultipartFormDataContent();
        formData.Add(content: new StreamContent(excelStream),
                    name: "file",
                    fileName: "abc.excel");


        // ==================================
        // Step-03: Execute the POST request
        // ==================================
        var httpClient = this._factory.CreateClient();
        var response = await httpClient.PostAsync("/products/import-excel", formData, this.CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var products = await this.ParseResponse<List<ImportProductModel>>(response);
        products.ShouldNotBeEmpty();
        products.Count.ShouldBe(numberOfProducts);
        products.ForEach(p =>
        {
            p.Code.ShouldNotBeNullOrWhiteSpace();
            p.Name.ShouldNotBeNullOrWhiteSpace();
        });
    }
}
