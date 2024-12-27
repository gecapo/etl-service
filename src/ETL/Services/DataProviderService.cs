namespace ETL.Services;

public sealed class DataProviderService(IFactory<IDataProvider> dataProviderStrategyFactory) : IDataProviderService
{
    private readonly IFactory<IDataProvider> _dataProviderStrategyFactory = dataProviderStrategyFactory;

    public DataProviderType? DataProviderType { get; set; }
    public DataProviderOptions? DataProviderOptions { get; set; } = null!;

    public async Task<RetrieveDataResult> RetrieveFirstOrDefaultAsync()
    {
        try
        {
            var retrieveStrategy = _dataProviderStrategyFactory.GetStrategy(DataProviderType.ToString());
            (string fileName, byte[] importData) = await retrieveStrategy.RetrieveDataFirstOrDefaultAsync(DataProviderOptions);

            if (importData.Length == 0)
                return new() { IsSuccess = false, Message = $"{nameof(DataProviderService)}:{nameof(RetrieveFirstOrDefaultAsync)}: No file was found!" };

            return new() { Data = importData, FileName = fileName, IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new() { IsSuccess = false, Message = $"{nameof(DataProviderService)}:{nameof(RetrieveFirstOrDefaultAsync)}: {ex.Message}"
            };
        }
    }

    public async Task<Result> HandleFailureAsync(string fileName)
    {
        try
        {
            var retrieveStrategy = _dataProviderStrategyFactory.GetStrategy(DataProviderType.ToString());
            await retrieveStrategy.HandleFailureAsync(fileName, DataProviderOptions);
            return new() { IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new() { IsSuccess = false, Message = $"{nameof(DataProviderService)}:{nameof(HandleFailureAsync)}: {ex.Message}" };
        }
    }

    public async Task<Result> HandleSuccessAsync(string fileName)
    {
        try
        {
            var retrieveStrategy = _dataProviderStrategyFactory.GetStrategy(DataProviderType.ToString());
            await retrieveStrategy.HandleSuccessAsync(fileName, DataProviderOptions);
            return new() { IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new() { IsSuccess = false, Message = $"{nameof(DataProviderService)}:{nameof(HandleSuccessAsync)}: {ex.Message}" };
        }
    }
}
