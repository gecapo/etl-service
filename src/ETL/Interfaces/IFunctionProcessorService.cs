namespace ETL.Interfaces;

public interface IFunctionProcessorService
{
    Task<Result> HandleFunction(FunctionType functionType, IImportPackageConfiguration importPackage, params object[] parameters);
}