namespace ETL.Interfaces;

public interface IProcessor : IStrategy
{
    Task RunAsync(IImportPackageConfiguration importPackage);
}