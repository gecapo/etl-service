namespace ETL.Interfaces;

public interface IDataProviderService
{
    DataProviderType? DataProviderType { get; set; }
    DataProviderOptions? DataProviderOptions { get; set; }

    Task<RetrieveDataResult> RetrieveFirstOrDefaultAsync();
    Task<Result> HandleFailureAsync(string fileName);
    Task<Result> HandleSuccessAsync(string fileName);
}
