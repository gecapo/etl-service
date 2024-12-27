namespace ETL.Services;

public sealed class ConfigurationProviderService : IConfigurationProviderService
{
    private readonly Dictionary<string, DataProviderOptions> Configuration = [];

    public ConfigurationProviderService(IConfigurationRepository configurationRepository)
    {
        var packageConfigurations = configurationRepository.GetAll();
        foreach (var packageConfiguration in packageConfigurations)
            Configuration.Add(packageConfiguration.PackageName!, packageConfiguration);
    }

    public DataProviderOptions GetByName(string name)
    {
        Configuration.TryGetValue(name, out var result);
        return result!;
    }

    public DataProviderOptions GetByType(Type type)
    {
        Configuration.TryGetValue(type.Name, out var result);
        return result!;
    }

    public List<string> GetAllConfigurationKeys() =>
    [
        .. Configuration.Where(x => x.Value.Enabled.GetValueOrDefault(false)).ToDictionary().Keys
    ];
}