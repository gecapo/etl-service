using CsvHelper;
using CsvHelper.Configuration;
using MassTransit.Internals;

public class CsvParseStrategy : Etl.Poco.Interfaces.IParser, IStrategy
{
    public bool IsHandler(string type) => type == ParserType.Csv.ToString();

    public async Task<IList<object>> ParseData(byte[] data, ParserOptions parserOptions)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        if (!string.IsNullOrWhiteSpace(parserOptions.CsvDelimiter))
        {
            config.Delimiter = parserOptions.CsvDelimiter;
        }

        if (parserOptions.ShouldSkipRecord != null)
        {
            config.ShouldSkipRecord = parserOptions.ShouldSkipRecord;
        }

        var encoding = parserOptions.FileEncoding;
        using var memoryStream = new MemoryStream(data);
        using var streamReader = new StreamReader(memoryStream, encoding);
        using var csvReader = new CsvReader(streamReader, config);

        return await csvReader.GetRecordsAsync(parserOptions.SourceEntityType!).ToListAsync();
    }
}