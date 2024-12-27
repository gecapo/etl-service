public sealed class FixedWidthParserOptions
{
    public bool ShouldSkipHeaderRow { get; set; }
    public string NewLine { get; set; } = Environment.NewLine;
    public int HeaderRowOffset { get; set; } = 1;
    public bool ShouldSkipFooterRow { get; set; }
    public int FooterRowOffset { get; set; } = 1;

    public Type ParserResultType { get; set; } = null!;
}