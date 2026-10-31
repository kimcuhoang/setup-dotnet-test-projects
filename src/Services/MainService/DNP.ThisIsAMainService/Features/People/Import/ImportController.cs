using CsvHelper;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;

namespace DNP.ThisIsAMainService.Features.People.Import;

[ApiController]
public partial class ImportController : ControllerBase
{
    [HttpPost("import-csv")]
    [ProducesResponseType<List<ImportPersonModel>>(StatusCodes.Status200OK)]
    public IActionResult DoImportCsv([FromForm] IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var csv = CsvReader.Create(stream, new CsvReaderOptions { Separator = ',' });
        var records = csv.GetRecords<ImportPersonModel>().ToList();
        return Ok(records);
    }

    [HttpPost("import-excel")]
    [ProducesResponseType<List<ImportPersonModel>>(StatusCodes.Status200OK)]
    public IActionResult DoImportExcel([FromForm] IFormFile file)
    {
        var records = new ExcelMapper(file.OpenReadStream()) { HeaderRow = true }.Fetch<ImportPersonModel>();
        return Ok(records.ToList());
    }
}
