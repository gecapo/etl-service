namespace ETL.Models.Common;

public sealed class TeamsWebhookConfiguration
{
    public string? BaseUri { get; set; }
    public string? FailedJobsUri { get; set; }
}