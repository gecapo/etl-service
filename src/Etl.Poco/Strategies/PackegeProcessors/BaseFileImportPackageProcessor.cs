using Etl.Poco.Interfaces;
using Etl.Poco.Services;

namespace Etl.Poco.Strategies.PackegeProcessors;

public abstract class BaseFileImportPackageProcessor(IServiceProvider serviceProvider)
{
    protected readonly IFileImportService _fileImportService = serviceProvider.GetService<IFileImportService>()!;
    protected readonly ISqlConnectionFactory _sqlConnectionFactory = serviceProvider.GetService<ISqlConnectionFactory>()!;
    protected readonly IMapper mapper = serviceProvider.GetService<IMapper>()!;

    protected readonly IDataProviderService _dataProviderService = new DataProviderService(serviceProvider.GetService<IFactory<IDataProvider>>()!);
    protected readonly IParserService _parserService = new ParserService(serviceProvider.GetService<IFactory<IParser>>()!, serviceProvider.GetService<IMapperService>()!);
    protected readonly IFunctionProcessorService _functionService = new FunctionProcessorService();
}
