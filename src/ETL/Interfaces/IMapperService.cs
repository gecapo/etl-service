namespace ETL.Interfaces;

public interface IMapperService
{
    Dictionary<Type, Delegate> TransformFunctionsDictionary { get; set; }
    MapperResult Map(object source, int fileImportId, Type sourceType, Type destinationType);
}
