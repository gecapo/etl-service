public sealed class FixedWitdhParseStrategy : IParser, IStrategy
{
    public bool IsHandler(string type) => type == ParserType.FixedWidth.ToString();

    public async Task<IList<object>> ParseData(byte[] data, ParserOptions parserOptions)
    {
        var encoding = parserOptions.FileEncoding;
        var content = encoding.GetString(data);

        var options = parserOptions.SourceEntityType.GetFixedWidthReportAttribute();

        FixedWidthParserOptions fixedWidthParserOptions = new()
        {
            ShouldSkipHeaderRow = options.ShouldSkipHeader,
            ShouldSkipFooterRow = options.ShouldSkipFooter,
            HeaderRowOffset = options.HeaderOffset,
            FooterRowOffset = options.FooterOffset,
            NewLine = options.NewLine,
            ParserResultType = parserOptions.SourceEntityType
        };

        return (await new FixedWidthParser().Parse(content, fixedWidthParserOptions)).ToList();
    }
}