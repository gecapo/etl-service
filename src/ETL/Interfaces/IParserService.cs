namespace ETL.Interfaces;

public interface IParserService
{
    ParserType? ParseType { get; set; }
    ParserOptions? ParserOptions { get; set; }

    Task<ParseResult> ParseData(int fileImportId, byte[] bytes);
}