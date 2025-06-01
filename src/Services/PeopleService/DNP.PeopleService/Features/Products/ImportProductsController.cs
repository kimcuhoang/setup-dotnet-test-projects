using CsvHelper;
using DNP.PeopleService.Features.Products.Models;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DNP.PeopleService.Features.Products;

[ApiController]
public class ImportProductsController : ControllerBase
{

    [HttpPost("/products/import-csv")]
    [ProducesResponseType<List<ImportProductModel>>(StatusCodes.Status200OK)]
    public IActionResult DoImportCsv(IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<ClassMapImportProductModel>();
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
