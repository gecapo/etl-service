using Renci.SshNet;

namespace ETL.Services;

//TODO:Refactor with factory
public sealed class TestFtpService(SftpConfiguration ftpConfiguration) : IFtpService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public SftpClient GetSftpClient()
    {
        using var key = new MemoryStream(Convert.FromBase64String(ftpConfiguration.Key!));
        AuthenticationMethod[] meths =
        [
            new PrivateKeyAuthenticationMethod(ftpConfiguration.Username,
                new PrivateKeyFile(key, ftpConfiguration.Password))
        ];

        var conInfo = new ConnectionInfo(ftpConfiguration.Hostname,
            ftpConfiguration.Port!.Value,
            ftpConfiguration.Username, meths);

        return new SftpClient(conInfo);
    }

    /// <summary>
    /// Saves file to the sftp
    /// </summary>
    /// <param name="filepath">The file path</param>
    /// <param name="filename">The file name</param>
    /// <param name="text">The file content</param>
    /// <param name="encoding">Default encoding is UTF-8</param>
    public async Task SaveFile(string filepath, string filename, string text, Encoding? encoding)
    {
        var textToByteArray = (encoding ?? Encoding.UTF8).GetBytes(text);
        await SaveFile(filepath, filename, textToByteArray);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="filename"></param>
    /// <param name="byteArray"></param>
    public async Task SaveFile(string filepath, string filename, byte[] byteArray)
    {
        using var client = GetSftpClient();
        client.Connect();

        await Task.Run(() =>
        {
            if (client.Get(filepath).IsDirectory)
            {
                using var stream = new MemoryStream(byteArray);
                client.UploadFile(stream, $"{filepath}/{filename}");
            }

            client.Disconnect();
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="originalFilePath"></param>
    /// <param name="newFilePath"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public async Task MoveFile(string originalFilePath, string newFilePath, string filename)
    {
        using var client = GetSftpClient();
        client.Connect();

        await Task.Run(() =>
        {
            var file = client.Get($"{originalFilePath}/{filename}");
            file.MoveTo($"{newFilePath}/{file.Name}");
            client.Disconnect();
        });
    }

    /// <summary>
    /// Open connection to the SFTP and get content of text file.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public async Task<string> GetStringFileContent(string filePath)
    {
        using var client = GetSftpClient();
        client.Connect();

        using var ms = new MemoryStream();
        client.DownloadFile(filePath, ms);

        client.Disconnect();

        var content = Encoding.UTF8.GetString(ms.ToArray());
        return await Task.FromResult(content);
    }

    public async Task<byte[]> GetFileContent(string filePath)
    {
        using var client = GetSftpClient();
        client.Connect();

        using var ms = new MemoryStream();
        client.DownloadFile(filePath, ms);

        client.Disconnect();

        return await Task.FromResult(ms.ToArray());
    }

    /// <summary>
    /// Open connection to the SFTP and delete file.
    /// </summary>
    /// <param name="originalFilePath"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public async Task DeleteFile(string originalFilePath, string filename)
    {
        using var client = GetSftpClient();
        client.Connect();

        await Task.Run(() =>
        {
            client.DeleteFile(originalFilePath);
            client.Disconnect();
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public async Task<bool> Exists(string filePath)
    {
        using var client = GetSftpClient();
        client.Connect();
        var exists = client.Exists(filePath);
        client.Disconnect();
        return await Task.FromResult(exists);
    }
}