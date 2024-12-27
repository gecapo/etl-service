namespace ETL.Extensions;

public static class MapperServiceExtensions
{
    public static IMapperService LoadConfigurationFromParser(this IMapperService mapperService, IParserService parser)
    {
        mapperService.TransformFunctionsDictionary = parser.ParserOptions.TransformFunctionsDictionary;
        return mapperService;
    }
}