namespace ETL.Extensions;

public static class IImportPackageConfigurationExtensions
{
    public static IImportPackageConfiguration UseProcessor(this IImportPackageConfiguration importPackageConfiguration, ProcessorType processorType)
    {
        importPackageConfiguration.ProcessorType = processorType;
        return importPackageConfiguration;
    }
    public static IImportPackageConfiguration UseParser(this IImportPackageConfiguration importPackageConfiguration, ParserType parserType, Func<ParserOptions, ParserOptions> options)
    {
        importPackageConfiguration.ParseType = parserType;
        ParserOptions parserOptions = new();
        parserOptions = options.Invoke(parserOptions);
        importPackageConfiguration.ParserOptions = parserOptions;
        return importPackageConfiguration;
    }
    public static IImportPackageConfiguration UseDataProvider(this IImportPackageConfiguration importPackageConfiguration, DataProviderType dataProviderType, Func<DataProviderOptions> options)
    {
        importPackageConfiguration.DataProviderType = dataProviderType;
        DataProviderOptions dataProviderOptions = options.Invoke();
        importPackageConfiguration.DataProviderOptions = dataProviderOptions;
        return importPackageConfiguration;
    }
    public static IImportPackageConfiguration HasFunction(this IImportPackageConfiguration importPackageConfiguration, FunctionType functionType, Delegate function)
    {
        importPackageConfiguration.Functions.Add(new() { FunctionType = functionType, Function = function });
        return importPackageConfiguration;
    }
}
