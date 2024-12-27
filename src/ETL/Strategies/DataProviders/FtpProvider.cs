using System.IO.Compression;
using System.Text.RegularExpressions;

namespace ETL.Strategies.DataProviders;

public sealed class FtpProvider(IFtpService ftpService) : IDataProvider, IStrategy
{
    public bool IsHandler(string type) => type == DataProviderType.Sftp.ToString();

    public async Task HandleFailureAsync(string fileName, DataProviderOptions options)
    {
        string basePath = options.ReportFolder;
        var fromPath = basePath + "in/";
        var toPath = basePath + "failed/";

        await ftpService.MoveFile(fromPath, toPath, fileName);
    }

    public async Task HandleSuccessAsync(string fileName, DataProviderOptions options)
    {
        string basePath = options.ReportFolder;
        var fromPath = basePath + "in/";
        var toPath = basePath + "imported/";

        await ftpService.MoveFile(fromPath, toPath, fileName);
    }

    public async Task<(string, byte[])> RetrieveDataFirstOrDefaultAsync(DataProviderOptions options)
    {
        string basePath = options.ReportFolder;
        var path = basePath + "in/";
        var mask = options.FileMask;

        var sftpClient = ftpService.GetSftpClient();
        sftpClient.Connect();

        var file = sftpClient.ListDirectory(path)
            .Where(file => !file.IsDirectory)
            .Where(x => Regex.IsMatch(x.Name, mask))
            .OrderBy(x => x.LastWriteTimeUtc)
            .FirstOrDefault();

        if (file == null)
            return await Task.FromResult<(string, byte[])>((string.Empty, Array.Empty<byte>()));

        byte[] fileBytes = sftpClient.ReadAllBytes(file.FullName);

        // Check if the file is a valid ZIP file by examining the file's first bytes
        if (IsZipFile(fileBytes))
        {
            // Unzip the file content
            using MemoryStream ms = new(fileBytes);
            using ZipArchive archive = new(ms, ZipArchiveMode.Read);
            var entry = archive.Entries.FirstOrDefault(); // assuming we process the first entry in the ZIP
            if (entry != null)
            {
                using (var entryStream = entry.Open())
                {
                    using (MemoryStream unzippedStream = new())
                    {
                        await entryStream.CopyToAsync(unzippedStream);
                        return (file.Name, unzippedStream.ToArray());
                    }
                }
            }
        }

        // If it's not a ZIP file, just return the file as is
        return (file.Name, fileBytes);
    }

    // Check if the first four bytes of the file match the ZIP signature (PK..)
    private static bool IsZipFile(byte[] fileBytes) =>
        fileBytes.Length > 4 && fileBytes[0] == 0x50 && fileBytes[1] == 0x4B && fileBytes[2] == 0x03 && fileBytes[3] == 0x04;
}