using System.Text.Json;
using Etl.Poco.Constants;
using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Strategies.Parsers;

public class JsonParseStrategy : IParser, IStrategy
{
    public bool IsHandler(string type) => type == ParserType.Json.ToString();

    public async Task<IList<object>> ParseData(byte[] data, ParserOptions parserOptions)
    {
        var encoding = parserOptions.FileEncoding;

        string jsonString;
        try
        {
            jsonString = Encoding.GetEncoding(encoding.CodePage).GetString(data);
        }
        catch (DecoderFallbackException ex)
        {
            throw new InvalidOperationException("Failed to decode the data using the specified encoding.", ex);
        }

        var sourceEntityType = parserOptions.SourceEntityType;

        if (sourceEntityType == null || !typeof(IEnumerable<object>).IsAssignableFrom(sourceEntityType))
        {
            throw new InvalidOperationException("Invalid source entity type for JSON deserialization.");
        }

        try
        {
            var deserializeMethod = typeof(JsonSerializer).GetMethod(
                nameof(JsonSerializer.Deserialize),
                new[] { typeof(string), typeof(JsonSerializerOptions) }
            )?.MakeGenericMethod(sourceEntityType);

            var result = deserializeMethod?.Invoke(null, [
                jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ]);

            return result as IList<object> ?? [];
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to deserialize the JSON data.", ex);
        }
    }
}