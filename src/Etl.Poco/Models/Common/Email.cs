namespace Etl.Poco.Models.Common;

public sealed record Email(string Subject, string ToEmails, string Body)
{
    public string? FromEmail { get; set; }
    public string? CcEmails { get; set; }
    public string? BccEmails { get; set; }

    public List<EmailAttachment> EmailAttachments { get; set; } = [];

    public sealed record EmailAttachment(byte[] Content, string FileName, string MediaType);
}