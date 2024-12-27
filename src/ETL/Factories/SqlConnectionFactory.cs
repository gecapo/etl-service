using Microsoft.Data.SqlClient;

namespace ETL.Factories;

public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public SqlConnection Create() => new(connectionString);
}