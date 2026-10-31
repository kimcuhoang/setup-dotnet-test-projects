using CsvHelper;
using Ganss.Excel;

namespace DNP.ThisIsAMainService.Features.People.Import;

[CsvRecord]
public partial class ImportPersonModel
{
    [Column(1)]
    public string Code { get; set; }
}