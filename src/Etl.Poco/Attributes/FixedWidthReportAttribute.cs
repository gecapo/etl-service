namespace Etl.Poco.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class FixedWidthReportAttribute(bool skipHeader = true,
    string? newLine = null,
    int headerOffset = 1,
    bool skipFooter = false,
    int footerOffset = 1) : Attribute
{
    public bool ShouldSkipHeader { get; set; } = skipHeader;
    public int HeaderOffset { get; set; } = headerOffset;
    public string NewLine { get; set; } = newLine ?? Environment.NewLine;
    public bool ShouldSkipFooter { get; set; } = skipFooter;
    public int FooterOffset { get; set; } = footerOffset;
}