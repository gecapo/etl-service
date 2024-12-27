using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Services;

public sealed class MapperService(IMapper mapper) : IMapperService
{
    private readonly string _fileImportPropertyName = "FileImportId";

    private readonly IMapper _mapper = mapper;

    public Dictionary<Type, Delegate> TransformFunctionsDictionary { get; set; } = [];

    public MapperResult Map(object source, int fileImportId, Type sourceType, Type destinationType)
    {
        var objectResult = MapObject(source, sourceType, destinationType);
        var mapperReulst = GetMapperResult(objectResult, fileImportId);
        return mapperReulst;
    }

    public MapperResult GetMapperResult(object mapperResult, int fileImportId)
    {
        if (mapperResult is null)
            return null;

        if (mapperResult is IEnumerable<object> mapperResultArray && mapperResultArray.Any())
        {
            var array = mapperResultArray.ToList();
            var propertyInfo = array.FirstOrDefault()!
                .GetType()
                .GetProperty(_fileImportPropertyName);

            if (propertyInfo == null) return new(mapperResultArray);

            foreach (var mapperResultArrayRecord in array)
                propertyInfo!.SetValue(mapperResultArrayRecord, fileImportId);

            return new(mapperResultArray);
        }

        mapperResult.GetType()
            .GetProperty(_fileImportPropertyName)
            ?.SetValue(mapperResult, fileImportId);

        return new(mapperResult);
    }

    private object MapObject(object source, Type sourceType, Type destinationType)
    {
        if (!TransformFunctionsDictionary.TryGetValue(destinationType, out var transformationDelegate))
            return _mapper.Map(source, sourceType, destinationType);

        var invokedFunction = transformationDelegate.DynamicInvoke([source]);
        if (invokedFunction is Task<object> awaitableFunction)
            return awaitableFunction.Result;

        return invokedFunction;
    }
}