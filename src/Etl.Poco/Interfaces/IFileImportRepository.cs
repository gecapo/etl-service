using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IFileImportRepository
{
    Task<FileImport?> GetByIdAsync(int id);
    Task<bool> ExistAsync(FileImport fileImport);
    Task<bool> ExistSameNameAsync(FileImport fileImport);
    Task<int> InsertAsync(FileImport fileImport);
    Task<bool> UpdateAsync(FileImport fileImport);
    Task<int> GetImportCounterAsync(string counterName);
    Task<bool> UpdateImportCounterAsync(string counterName, int counter);
    Task<FileImport?> GetLastSuccessfulFileImportByType(int type);
}