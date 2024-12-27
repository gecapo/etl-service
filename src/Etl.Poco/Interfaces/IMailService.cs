using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IMailService
{
    Task SendEmail(Email email);
}