namespace ETL.Models.Common;

public sealed class SftpConfiguration
{
    public string? Hostname { get; set; } = null!;
    public int? Port { get; set; } = null!;
    public string? Username { get; set; } = null!;
    public string? Key { get; set; } = null!;
    public string? Password { get; set; } = null!;
}