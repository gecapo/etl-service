using Etl.Poco.Attributes;

namespace Etl.Poco.Models.ReportModels;

[FixedWidthReport(ShouldSkipHeader = true, NewLine = "\n", ShouldSkipFooter = true)]
public class TestModelDto
{
    [FixedWidthColumn(1, 19)]
    public string? TestFieldOne { get; set; }

    [FixedWidthColumn(21, 19)]
    public string? TestFieldTwo { get; set; }
}

public class TestModel : EtlEntity
{
    public string? TestFieldOne { get; set; }

    public string? TestFieldTwo { get; set; }
}

public abstract class EtlEntity
{
    public int FileImportId { get; set; }
}