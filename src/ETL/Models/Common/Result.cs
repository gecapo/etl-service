namespace ETL.Models.Common;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }

    public static implicit operator Result(bool status) => new() { IsSuccess = status };
}
