namespace Etl.Poco.Models.Common;

public sealed class ParseResult : Result
{
    public int RowsCount { get; set; } = 0!;
    public HashSet<List<object>> ParsedData { get; set; } = [];
}
