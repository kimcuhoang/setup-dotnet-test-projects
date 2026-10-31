using CsvHelper;
using DNP.ThisIsAMainService.Features.Products.Models;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;

namespace DNP.ThisIsAMainService.Features.Products;

[ApiController]
public class ImportProductsController : ControllerBase
{

    [HttpPost("/products/import-csv")]
    [ProducesResponseType<List<ImportProductModel>>(StatusCodes.Status200OK)]
    public IActionResult DoImportCsv(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var csv = CsvReader.Create(stream, new CsvReaderOptions { Separator = ',', MissingField = CsvMissingFieldHandling.Throw });
        var records = csv.GetRecords<ImportProductModel>().ToList();
        return Ok(records);
    }

    [HttpPost("/products/import-excel")]
    [ProducesResponseType<List<ImportProductModel>>(StatusCodes.Status200OK)]
    public IActionResult DoImportExcel([FromForm] IFormFile file)
    {
        var records = new ExcelMapper(file.OpenReadStream()) { HeaderRow = true }.Fetch<ImportProductModel>();
        return Ok(records.ToList());
    }
}
