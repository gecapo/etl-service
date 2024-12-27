using Etl.Poco.Constants;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IParserService
{
    ParserType? ParseType { get; set; }
    ParserOptions? ParserOptions { get; set; }

    Task<ParseResult> ParseData(int fileImportId, byte[] bytes);
}