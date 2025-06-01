using CsvHelper;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DNP.PeopleService.Features.People;

[ApiController]
public class ImportController : ControllerBase
{
    public class PersonCode
    {
        [Column(1)]
        public string Code { get; set; }
    }

    [HttpPost("import-csv")]
    [ProducesResponseType<List<PersonCode>>(StatusCodes.Status200OK)]
    public IActionResult DoImportCsv([FromForm] IFormFile file)
    {
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
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
