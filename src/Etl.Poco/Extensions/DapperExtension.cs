using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Etl.Poco.Extensions;

public static class DapperExtension
{
    public static async Task<int> BulkInsertToDatabase<T>(this SqlConnection connection,
        ICollection<T> entities,
        string? destinationTableName = null,
        int bulkCopyTimeoutSeconds = 300)
    {
        if (!entities.Any()) return entities.Count();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        var entityType = entities.FirstOrDefault()?.GetType();
        var entityTableNameAttribute = entityType
            ?.GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), true)
            .Select(x => (x as Dapper.Contrib.Extensions.TableAttribute)?.Name)
            .FirstOrDefault()!
            .ToString();

        Debug.Assert(entityType != null, nameof(entityType) + " != null");
        var dataTable = entities.GetDataTableFromEntities(entityType);
        using var bulkCopy = new SqlBulkCopy(connection);
        bulkCopy.BulkCopyTimeout = bulkCopyTimeoutSeconds;
        bulkCopy.DestinationTableName = entityTableNameAttribute;

        foreach (DataColumn column in dataTable.Columns)
        {
            var property = entityType.GetProperty(column.ColumnName);
            if (property != null)
            {
                // Look for the ColumnAttribute on the property
                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                var columnName =
                    columnAttribute?.Name ??
                    column.ColumnName; // Use ColumnAttribute's Name or fallback to the property name
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(column.ColumnName, columnName));
            }
            else
            {
                bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(column.ColumnName,
                    column.ColumnName)); // Fallback if no property found
            }
        }

        await bulkCopy.WriteToServerAsync(dataTable);

        return entities.Count();
    }

    private static DataTable GetDataTableFromEntities<T>(this IEnumerable<T> entities, Type entityType)
    {
        var dataTable = new DataTable();
        foreach (var property in entityType.GetProperties())
        {
            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            DataColumn dataColumn = new(property.Name, type);
            dataTable.Columns.Add(dataColumn);
        }

        foreach (var entity in entities)
        {
            var dataRow = dataTable.NewRow();
            foreach (var property in entityType.GetProperties())
            {
                dataRow[property.Name] = property.GetValue(entity) ?? DBNull.Value;
            }

            dataTable.Rows.Add(dataRow);
        }

        return dataTable;
    }
}