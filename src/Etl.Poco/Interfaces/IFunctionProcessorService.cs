using Etl.Poco.Constants;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IFunctionProcessorService
{
    Task<Result> HandleFunction(FunctionType functionType, IImportPackageConfiguration importPackage, params object[] parameters);
}