using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface ITeamsWebHookService
{
    Task SendMessageAsync(TeamsWebhookFailedNotificationMessage message);
    void SendMessage(TeamsWebhookFailedNotificationMessage message);
}
