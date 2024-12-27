
public sealed class FixedWidthParser
{
    public async Task<IEnumerable<object>> Parse(string content, FixedWidthParserOptions options)
    {
        Type parserResultType = options.ParserResultType;
        var columns = FixedWidthParserExtensions.GetAllFixedWidthColumns(parserResultType);

        var contentRows = content.Split(options.NewLine);

        if (options.ShouldSkipHeaderRow && contentRows.Length > 0)
            contentRows = contentRows.Skip(options.HeaderRowOffset).ToArray();

        if (options.ShouldSkipFooterRow && contentRows.Length > 0)
            contentRows = contentRows.Take(contentRows.Length - options.FooterRowOffset).ToArray();

        var result = new List<object>();
        foreach (var row in contentRows)
        {
            var entityInstance = Activator.CreateInstance(parserResultType);
            foreach (var column in columns)
            {
                if (row.Length < column!.StartIndex + column.Length)
                    throw new ArgumentOutOfRangeException($"Row is too short for column '{column.PropertyName}'.");

                var value = row.Substring(column.StartIndex, column.Length);
                if (column.ShouldTrim)
                    value = value.Trim();

                var propertyInfo = parserResultType.GetProperty(column.PropertyName);
                if (propertyInfo != null)
                {
                    Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                    // Check if value is null or empty and if the property is a nullable type
                    object? convertedValue = (value == null || string.IsNullOrEmpty(value.ToString()))
                        ? null
                        : Convert.ChangeType(value, propertyType);

                    propertyInfo.SetValue(entityInstance, convertedValue);
                }
            }

            result.Add(entityInstance!);
        }

        return await Task.FromResult(result);
    }
}
