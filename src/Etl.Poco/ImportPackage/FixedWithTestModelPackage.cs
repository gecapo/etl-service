using Etl.Poco.Constants;
using Etl.Poco.Interfaces;
using Etl.Poco.Models.ReportModels;

namespace Etl.Poco.ImportPackage;

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
            .UseProcessor(Constants.ProcessorType.Generic);

    public async Task PostProcess(params object[] values) { }
}

public sealed class TestPackageProfile : Profile
{
    public TestPackageProfile()
    {
        CreateMap<TestModelDto, TestModelDto>().ReverseMap();
    }
}