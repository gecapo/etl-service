namespace ETL.Models.Common;

public sealed class RetrieveDataResult : Result
{
    public string? FileName { get; set; }
    public byte[]? Data { get; set; }
}
