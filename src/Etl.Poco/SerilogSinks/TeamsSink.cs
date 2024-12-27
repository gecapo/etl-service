using Serilog.Events;
using Serilog.Formatting.Display;

public sealed class TeamsSink(IFormatProvider formatProvider, ITeamsWebHookService _teamsWebHookService) : Serilog.Core.ILogEventSink
{
    private readonly MessageTemplateTextFormatter _formatter = new("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", formatProvider);
    public void Emit(LogEvent logEvent)
    {
        if (logEvent.Level != LogEventLevel.Error || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            return;

        using var writer = new StringWriter();
        _formatter.Format(logEvent, writer);
        var message = writer.ToString();

        _teamsWebHookService.SendMessage(new("Etl.Poco", message));
    }
}
