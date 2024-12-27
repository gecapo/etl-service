using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Services;

public sealed class FileImportService(IFileImportRepository _fileImportRepository) : IFileImportService
{
    public async Task<FileImportResult> InsertNewFileImport(string name, int type, bool isFileNameDuplicationAllowed = false)
        => await InsertNewFileImport(new(name, type), isFileNameDuplicationAllowed);

    public async Task<FileImportResult> InsertNewFileImport(FileImport fileImport, bool isFileNameDuplicationAllowed = false)
    {
        var id = await _fileImportRepository.InsertAsync(fileImport);
        //check if this will be done under the hood
        fileImport.Id = id;

        bool success = true;
        if (!isFileNameDuplicationAllowed)
            success = !await _fileImportRepository.ExistSameNameAsync(fileImport);

        if (!success)
        {
            fileImport.Note = "Duplicated file name!";
            fileImport.Status = FileImportStatus.Failed;
            await _fileImportRepository.UpdateAsync(fileImport);
        }
        else
        {
            fileImport.Status = FileImportStatus.InProgress;
            fileImport.LastModified = DateTime.UtcNow;
            await _fileImportRepository.UpdateAsync(fileImport);
        }

        return new FileImportResult() { FileImport = fileImport, IsSuccess = success, Message = fileImport.Note };
    }

    public async Task<Result> UpdateFailure(FileImport fileImport, Exception e)
    {
        fileImport.Status = FileImportStatus.Failed;
        fileImport.Note = $"{e.Message} with {e.StackTrace}";
        return await _fileImportRepository.UpdateAsync(fileImport);
    }

    public async Task<Result> UpdateSuccessful(FileImport fileImport, int count)
    {
        fileImport.RowsImported = count;
        fileImport.Status = FileImportStatus.Completed;
        fileImport.DateCompleted = DateTime.UtcNow;
        fileImport.LastModified = DateTime.UtcNow;
        return await _fileImportRepository.UpdateAsync(fileImport);
    }

    public async Task<int> GetImportCounter(string counterName) =>
        await _fileImportRepository.GetImportCounterAsync(counterName);

    public async Task<Result> SetImportCounter(string counterName, int value) =>
        await _fileImportRepository.UpdateImportCounterAsync(counterName, value);

    public async Task<FileImport?> GetLastSuccessfulFileImportByType(int type) =>
        await _fileImportRepository.GetLastSuccessfulFileImportByType(type);
}
