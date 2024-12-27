namespace ETL.ImportPackage;

public sealed class FixedWithTestModelPackage : ImportPackage
{
    public FixedWithTestModelPackage(IServiceProvider serviceProvider) : base(serviceProvider)
        => this.UseDataProvider(Constants.DataProviderType.Sftp, () =>
                serviceProvider.GetRequiredService<IConfigurationProviderService>()
                    .GetByName(nameof(FixedWithTestModelPackage)))
            .UseParser(ParserType.FixedWidth, (options) =>
                options.HasParserResult<TestModelDto>()
                    .HasMapperDataTransform<TestModelDto>())
            .HasFunction(FunctionType.OnAfterInsert, PostProcess)
            .UseProcessor(ProcessorType.Generic);

    public async Task PostProcess(params object[] values) { }
}

public sealed class TestPackageProfile : Profile
{
    public TestPackageProfile()
    {
        CreateMap<TestModelDto, TestModelDto>().ReverseMap();
    }
}