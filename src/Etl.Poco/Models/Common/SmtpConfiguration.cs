namespace Etl.Poco.Models.Common;

public sealed class SmtpConfiguration
{
    public string? Hostname { get; set; } = null!;
    public int? Port { get; set; } = null!;
    public string? Username { get; set; } = null!;
    public string? TargetName { get; set; } = null!;
    public string? Password { get; set; } = null!;
}