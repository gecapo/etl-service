namespace ETL.Models.Common;

public class DataProviderOptions
{
    public int Id { get; set; }
    public int? ImportType { get; set; } = null!;
    public bool? Enabled { get; set; } = null!;
    public string? PackageName { get; set; } = null!;

    #region SftpProvider
    public string? ReportFolder { get; set; }
    public SftpBehavior AfterImportBehavior { get; set; }
    public string? ArchiveFolder { get; set; }
    public SftpBehavior AfterErrorFolder { get; set; }
    public string? ErrorFolder { get; set; }
    public string? FileMask { get; set; }
    #endregion

    #region SqlProvider
    public string? RetrieveDataQuery { get; set; }
    #endregion
}

public enum SftpBehavior
{
    Delete, Move
}