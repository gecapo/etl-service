using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IConfigurationRepository
{
    Task<DataProviderOptions> GetByIdAsync(int id);
    Task<List<DataProviderOptions>> GetAllAsync();
    DataProviderOptions GetById(int id);
    List<DataProviderOptions> GetAll();
}
