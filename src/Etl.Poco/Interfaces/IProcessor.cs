namespace Etl.Poco.Interfaces;

public interface IProcessor : IStrategy
{
    Task RunAsync(IImportPackageConfiguration importPackage);
}