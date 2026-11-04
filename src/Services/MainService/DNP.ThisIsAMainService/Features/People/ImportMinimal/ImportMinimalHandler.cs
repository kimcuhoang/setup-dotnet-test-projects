using CsvHelper;
using DNP.ThisIsAMainService.Common.ResultPattern;
using Ganss.Excel;
using Microsoft.AspNetCore.Mvc;

namespace DNP.ThisIsAMainService.Features.People.ImportMinimal;

public record ImportMinimalPayload(
    [FromForm] string NameOfIMport,
    IFormFile  ImportFile
);

[CsvRecord]
public partial class ImportMinimalPersonModel
{
    [Column(1)]
    public string Code { get; set; }
}

public class ImportMinimalHandler()
{
    public async Task<Result<List<ImportMinimalPersonModel>>> Handle(ImportMinimalPayload payload, CancellationToken cancellationToken)
    {
        using var stream = payload.ImportFile.OpenReadStream();
        using var csv = CsvReader.Create(stream, new CsvReaderOptions { Separator = ',' });
        var records = csv.GetRecords<ImportMinimalPersonModel>().ToList();
        return Result<List<ImportMinimalPersonModel>>.Success(records);
    }
}