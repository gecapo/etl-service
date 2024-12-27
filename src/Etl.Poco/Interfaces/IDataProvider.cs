using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IDataProvider : IStrategy
{
    Task<(string, byte[])> RetrieveDataFirstOrDefaultAsync(DataProviderOptions importPackage);
    Task HandleSuccessAsync(string retrievalIndicator, DataProviderOptions importPackage);
    Task HandleFailureAsync(string retrievalIndicator, DataProviderOptions importPackage);
}
