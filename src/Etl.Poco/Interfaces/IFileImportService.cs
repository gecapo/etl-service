using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IFileImportService
{
    Task<FileImportResult> InsertNewFileImport(string name, int type, bool isFileNameDuplicationAllowed = false);
    Task<Result> UpdateFailure(FileImport fileImport, Exception e);
    Task<Result> UpdateSuccessful(FileImport fileImport, int count);
    Task<FileImport?> GetLastSuccessfulFileImportByType(int type);
}