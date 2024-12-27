using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace ETL.Services;

public sealed class TestMailService(SmtpConfiguration _mailConfiguration) : IMailService
{
    public async Task SendEmail(Email mail)
    {
        using MailMessage msg = new();
        msg.From = new MailAddress(mail.FromEmail ?? "georgekalinkov@gmail.com");

        msg.To.Add("georgekalinkov@gmail.com");
        if (!string.IsNullOrWhiteSpace(mail.CcEmails))

            foreach (var toMail in mail.CcEmails.Split(";", StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries))
                msg.CC.Add(new MailAddress(toMail));

        if (!string.IsNullOrWhiteSpace(mail.BccEmails))
            foreach (var toMail in mail.BccEmails.Split(";", StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries))
                msg.Bcc.Add(new MailAddress(toMail));

        msg.Subject = mail.Subject;
        msg.Body = string.IsNullOrWhiteSpace(mail.Body) ? "This is an automated message, so please refrain from replying." : mail.Body;
        msg.IsBodyHtml = true;

        foreach (var file in mail.EmailAttachments!)
        {
            using var memoryStream = new MemoryStream(file.Content);
            using var attachment = new Attachment(memoryStream, file.FileName, file.MediaType);
            ContentDisposition disposition = attachment.ContentDisposition!;
            disposition.CreationDate = DateTime.UtcNow;
            disposition.ModificationDate = DateTime.UtcNow;
            disposition.ReadDate = DateTime.UtcNow;
            msg.Attachments.Add(attachment);
        }

        int count = 0;
        do
        {
            SmtpClient client = new()
            {
                Host = _mailConfiguration.Hostname!,
                Port = _mailConfiguration.Port!.Value,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_mailConfiguration.Username, _mailConfiguration.Password),
                TargetName = _mailConfiguration.TargetName,
                EnableSsl = true,
            };

            try
            {
                count++;
                await client.SendMailAsync(msg);
                client.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            client.Dispose();
            Random rnd = new();
            var delay = rnd.Next(1, 11);
            var randomWait = Task.Delay(delay * 1000);
            randomWait.Wait();

        } while (count < 15);
    }
}
