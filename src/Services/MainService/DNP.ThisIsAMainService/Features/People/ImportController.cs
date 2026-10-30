using CsvHelper;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;

namespace DNP.ThisIsAMainService.Features.People;

[ApiController]
public partial class ImportController : ControllerBase
{
    [CsvRecord]
    public partial class PersonCode
    {
        [Column(1)]
        public string Code { get; set; }
    }

    [HttpPost("import-csv")]
    [ProducesResponseType<List<PersonCode>>(StatusCodes.Status200OK)]
    public IActionResult DoImportCsv([FromForm] IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var csv = CsvReader.Create(stream, new CsvReaderOptions { Separator = ',' });
        var records = csv.GetRecords<PersonCode>().ToList();
        return Ok(records);
    }

    [HttpPost("import-excel")]
    [ProducesResponseType<List<PersonCode>>(StatusCodes.Status200OK)]
    public IActionResult DoImportExcel([FromForm] IFormFile file)
    {
        var records = new ExcelMapper(file.OpenReadStream()) { HeaderRow = true }.Fetch<PersonCode>();
        return Ok(records.ToList());
    }
}
