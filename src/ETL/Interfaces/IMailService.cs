namespace ETL.Interfaces;

public interface IMailService
{
    Task SendEmail(Email email);
}