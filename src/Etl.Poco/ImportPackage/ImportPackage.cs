using Etl.Poco.Constants;
using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;

namespace Etl.Poco.ImportPackage;

public abstract class ImportPackage(IServiceProvider serviceProvider) : IImportPackageConfiguration
{
    protected readonly IServiceProvider _serviceProvider = serviceProvider;

    public DataProviderType? DataProviderType { get; set; } = null!;
    public DataProviderOptions? DataProviderOptions { get; set; } = null!;

    public ParserType? ParseType { get; set; } = null!;
    public ParserOptions? ParserOptions { get; set; } = null!;

    public ProcessorType ProcessorType { get; set; } = ProcessorType.Generic;
    public List<ProcessFunction> Functions { get; set; } = [];
}