namespace ETL.Strategies.DataProviders;

public abstract class BaseApiStrategy() : IDataProvider, IStrategy
{
    public abstract bool IsHandler(string type);

    public async Task HandleFailureAsync(string fileName, DataProviderOptions parameters) => await Task.CompletedTask;
    public async Task HandleSuccessAsync(string fileName, DataProviderOptions parameters) => await Task.CompletedTask;

    public abstract Task<(string, byte[])> RetrieveDataFirstOrDefaultAsync(DataProviderOptions parameters);
}
