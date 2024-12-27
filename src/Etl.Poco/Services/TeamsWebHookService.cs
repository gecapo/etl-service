using System.Net.Http.Json;
using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Services;

public sealed class TeamsWebHookService(IHttpClientFactory _httpClientFactory, TeamsWebhookConfiguration _configuration) : ITeamsWebHookService
{
    public async Task SendMessageAsync(TeamsWebhookFailedNotificationMessage message)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(TeamsWebHookService));
        await httpClient.PostAsJsonAsync(_configuration.FailedJobsUri, message);
    }

    public void SendMessage(TeamsWebhookFailedNotificationMessage message) => SendMessageAsync(message).Wait();
}
