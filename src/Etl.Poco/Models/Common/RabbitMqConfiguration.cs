namespace Etl.Poco.Models.Common;

public sealed class RabbitMqConfiguration
{
    public string? Host { get; set; } = null!;
    public int? Port { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public Dictionary<string, string>? Endpoints { get; set; } = [];
}
