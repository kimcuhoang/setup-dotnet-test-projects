using DNP.PeopleService.Features.Products.Models;
using NPOI.XSSF.UserModel;
using System.Text;

namespace DNP.PeopleService.Tests.xUnitV3.TestProducts;
//public class TestImportProducts(ServiceTestAssemblyFixture testCollectionFixture, ITestOutputHelper testOutputHelper)
//    : ServiceTestBase(testCollectionFixture, testOutputHelper)

public class TestImportProducts(TestCollectionFixture testCollectionFixture, 
                                ITestOutputHelper testOutputHelper, 
                                ITestContextAccessor testContextAccessor)
    : IntegrationTestBase(testCollectionFixture, testOutputHelper, testContextAccessor)
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
                    Faker.Random.AlphaNumeric(10).ToUpper(),
                    Faker.Commerce.ProductName()
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
        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/products/import-csv", formData, CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var products = await ParseResponse<List<ImportProductModel>>(response);

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
            dataRow.CreateCell(0).SetCellValue(Faker.Random.AlphaNumeric(10).ToUpper());
            dataRow.CreateCell(1).SetCellValue(Faker.Commerce.ProductName());
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
        var httpClient = Factory.CreateClient();
        var response = await httpClient.PostAsync("/products/import-excel", formData, CancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var products = await ParseResponse<List<ImportProductModel>>(response);
        products.ShouldNotBeEmpty();
        products.Count.ShouldBe(numberOfProducts);
        products.ForEach(p =>
        {
            p.Code.ShouldNotBeNullOrWhiteSpace();
            p.Name.ShouldNotBeNullOrWhiteSpace();
        });
    }
}
