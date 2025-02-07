using CsvHelper.Configuration;
using Ganss.Excel;

namespace DNP.PeopleService.Features.Products.Models;

public class ImportProductModel
{
    [Column("Product Code")]
    public string Code { get; set; }

    [Column("Product Name")]
    public string Name { get; set; }
}

public class ClassMapImportProductModel : ClassMap<ImportProductModel>
{
    public ClassMapImportProductModel()
    {
        Map(m => m.Code).Name("Product Code");
        Map(m => m.Name).Name("Product Name");
    }
}
