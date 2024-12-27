using Dapper;
using Etl.Poco.Constants;
using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;
using Microsoft.Data.SqlClient;

namespace Etl.Poco.Strategies.DataProviders;

public sealed class SqlProvider(ISqlConnectionFactory sqlConnectionFactory) : IDataProvider, IStrategy
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory = sqlConnectionFactory;

    public bool IsHandler(string type) => type == DataProviderType.Sql.ToString();

    public async Task HandleFailureAsync(string counter, DataProviderOptions parameters) => await Task.CompletedTask;
    public async Task HandleSuccessAsync(string counter, DataProviderOptions parameters)
    {
        var query = "[Staging].[SetCounter]";
        using SqlConnection connection = _sqlConnectionFactory.Create();
        await connection.ExecuteAsync(query, new { ImportName = parameters.PackageName, Value = counter }, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<(string, byte[])> RetrieveDataFirstOrDefaultAsync(DataProviderOptions parameters)
    {
        throw new NotImplementedException();
    }
}
