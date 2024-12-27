namespace ETL.Interfaces;

public interface IConfigurationProviderService
{
    DataProviderOptions GetByName(string name);
    DataProviderOptions GetByType(Type type);
    List<string> GetAllConfigurationKeys();
}
