using Etl.Poco.Constants;
using Etl.Poco.Interfaces;
using Etl.Poco.Models.ReportModels;

namespace Etl.Poco.ImportPackage;

public sealed class FixedWidthWithCustomMapPackage : ImportPackage
{
    public FixedWidthWithCustomMapPackage(IServiceProvider serviceProvider) : base(serviceProvider)
        => this.UseDataProvider(Constants.DataProviderType.Sftp, () =>
                serviceProvider.GetRequiredService<IConfigurationProviderService>()
                    .GetByName(nameof(FixedWidthWithCustomMapPackage)))
        .UseParser(ParserType.FixedWidth,
            (options) => options.HasParserResult<List<TestModelDto>>()
                .HasFunctionDataTransform<List<TestModel>>(Map))
        .UseProcessor(Constants.ProcessorType.Generic);

    public List<TestModel> Map(List<TestModelDto> values)
    {
        return values.Select(x => new TestModel()).ToList();
    }
}
