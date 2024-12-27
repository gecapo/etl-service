namespace ETL.ImportPackage;

public sealed class FixedWidthWithCustomMapPackage : ImportPackage
{
    public FixedWidthWithCustomMapPackage(IServiceProvider serviceProvider) : base(serviceProvider)
        => this.UseDataProvider(Constants.DataProviderType.Sftp, () =>
                serviceProvider.GetRequiredService<IConfigurationProviderService>()
                    .GetByName(nameof(FixedWidthWithCustomMapPackage)))
        .UseParser(ParserType.FixedWidth,
            (options) => options.HasParserResult<List<TestModelDto>>()
                .HasFunctionDataTransform<List<TestModel>>(Map))
        .UseProcessor(ProcessorType.Generic);

    public List<TestModel> Map(List<TestModelDto> values)
    {
        return values.Select(x => new TestModel()).ToList();
    }
}
