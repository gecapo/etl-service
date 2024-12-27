using Etl.Poco.Interfaces;
using Microsoft.Data.SqlClient;

namespace Etl.Poco.Factories;

public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public SqlConnection Create() => new(connectionString);
}