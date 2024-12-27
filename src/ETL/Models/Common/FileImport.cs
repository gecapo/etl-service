
namespace ETL.Models.Common;

public sealed class FileImport
{
    public FileImport(string fileNmae, int type)
    {
        FileName = fileNmae;
        Type = type;
    }

    public int Id { get; set; }
    public string? FileName { get; set; }
    public int Type { get; set; }
    public string? Note { get; set; }
    public FileImportStatus? Status { get; set; }
    public DateTime LastModified { get; internal set; }
    public int RowsImported { get; internal set; }
    public DateTime DateCompleted { get; internal set; }
}