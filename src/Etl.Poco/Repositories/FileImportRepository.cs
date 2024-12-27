using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Repositories;

public sealed class FileImportRepository(ISqlConnectionFactory _connectionFactory) : IFileImportRepository
{
    public Task<bool> ExistAsync(FileImport fileImport)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistSameNameAsync(FileImport fileImport)
    {
        throw new NotImplementedException();
    }

    public Task<FileImport?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetImportCounterAsync(string counterName)
    {
        throw new NotImplementedException();
    }

    public Task<FileImport?> GetLastSuccessfulFileImportByType(int type)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertAsync(FileImport fileImport)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(FileImport fileImport)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateImportCounterAsync(string counterName, int counter)
    {
        throw new NotImplementedException();
    }
}