using Microsoft.Data.SqlClient;

namespace ETL.Interfaces;

public interface ISqlConnectionFactory
{
    SqlConnection Create();
}