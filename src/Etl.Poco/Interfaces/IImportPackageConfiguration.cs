using Etl.Poco.Constants;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IImportPackageConfiguration
{
    DataProviderType? DataProviderType { get; set; }
    DataProviderOptions? DataProviderOptions { get; set; }

    ParserType? ParseType { get; set; }
    ParserOptions? ParserOptions { get; set; }

    ProcessorType ProcessorType { get; set; }

    List<ProcessFunction> Functions { get; set; }
}
