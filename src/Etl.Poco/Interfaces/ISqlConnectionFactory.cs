using Microsoft.Data.SqlClient;

namespace Etl.Poco.Interfaces;

public interface ISqlConnectionFactory
{
    SqlConnection Create();
}