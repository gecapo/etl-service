using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IConfigurationProviderService
{
    DataProviderOptions GetByName(string name);
    DataProviderOptions GetByType(Type type);
    List<string> GetAllConfigurationKeys();
}
