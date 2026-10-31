using CsvHelper;
using Ganss.Excel;

namespace DNP.ThisIsAMainService.Features.Products.Import;

[CsvRecord]
public partial class ImportProductModel
{
    [Column("Product Code")]
    [CsvColumn("Product Code")]
    public string Code { get; set; }

    [Column("Product Name")]
    [CsvColumn("Product Name")]
    public string Name { get; set; }
}
