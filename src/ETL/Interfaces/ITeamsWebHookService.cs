namespace ETL.Interfaces;

public interface ITeamsWebHookService
{
    Task SendMessageAsync(TeamsWebhookFailedNotificationMessage message);
    void SendMessage(TeamsWebhookFailedNotificationMessage message);
}
