using Etl.Poco.Interfaces;

namespace Etl.Poco.Extensions;

public static class MapperServiceExtensions
{
    public static IMapperService LoadConfigurationFromParser(this IMapperService mapperService, IParserService parser)
    {
        mapperService.TransformFunctionsDictionary = parser.ParserOptions.TransformFunctionsDictionary;
        return mapperService;
    }
}