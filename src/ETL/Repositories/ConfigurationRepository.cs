using Dapper;

namespace ETL.Repositories;

public sealed class ConfigurationRepository(ISqlConnectionFactory sqlConnectionFactory) : IConfigurationRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    public async Task<DataProviderOptions> GetByIdAsync(int id)
    {
        await using var connection = _sqlConnectionFactory.Create();
        return await connection.QueryFirstOrDefaultAsync<DataProviderOptions>("");
    }

    public async Task<List<DataProviderOptions>> GetAllAsync()
    {
        await using var connection = _sqlConnectionFactory.Create();
        var configuration = await connection.QueryAsync<DataProviderOptions>("");
        return configuration.ToList();
    }

    public DataProviderOptions GetById(int id)
    {
        using var connection = _sqlConnectionFactory.Create();
        return connection.QueryFirstOrDefault<DataProviderOptions>("");
    }

    public List<DataProviderOptions> GetAll()
    {
        using var connection = _sqlConnectionFactory.Create();
        return connection.Query<DataProviderOptions>("").ToList();
    }
}