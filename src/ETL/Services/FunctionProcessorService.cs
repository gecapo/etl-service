namespace ETL.Services;

public sealed class FunctionProcessorService : IFunctionProcessorService
{
    public async Task<Result> HandleFunction(FunctionType functionType, IImportPackageConfiguration importPackage, params object[] parameters)
    {
        var functionsToRun = importPackage.Functions
            .Where(x => x.FunctionType == functionType)
            .Select(x => x.Function)
            .ToList();

        Result result = new() { IsSuccess = true };
        try
        {
            foreach (var function in functionsToRun)
            {
                var invokedFunction = function.DynamicInvoke([parameters]);
                if (invokedFunction is Task awaitableFunction)
                    await awaitableFunction;
            }
        }
        catch (Exception ex)
        {
            result = new() { IsSuccess = false, Message = $"{nameof(FunctionProcessorService)}:{nameof(HandleFunction)}: {ex.Message}" };
        }

        return result;
    }
}
