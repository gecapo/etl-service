using Renci.SshNet;

namespace ETL.Interfaces;

public interface IFtpService
{
    SftpClient GetSftpClient();
    Task SaveFile(string filepath, string filename, string text, Encoding encoding);
    Task SaveFile(string filepath, string filename, byte[] byteArray);
    Task MoveFile(string originalFilePath, string newFilePath, string filename);
    Task<string> GetStringFileContent(string filePath);
    Task<byte[]> GetFileContent(string filePath);
    Task DeleteFile(string originalFilePath, string filename);
    Task<bool> Exists(string filePath);
}