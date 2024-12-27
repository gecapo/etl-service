using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IMapperService
{
    Dictionary<Type, Delegate> TransformFunctionsDictionary { get; set; }
    MapperResult Map(object source, int fileImportId, Type sourceType, Type destinationType);
}
