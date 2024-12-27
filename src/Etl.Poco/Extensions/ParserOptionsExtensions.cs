using Etl.Poco.Models.Common;

namespace Etl.Poco.Extensions;

public static class ParserOptionsExtensions
{
    public static ParserOptions HasParserResult<T>(this ParserOptions parserOptions)
    {
        parserOptions.SetParserResult<T>();
        return parserOptions;
    }

    public static ParserOptions HasMapperDataTransform<T>(this ParserOptions parserOptions)
    {
        parserOptions.SetDataTransform<T>();
        return parserOptions;
    }

    public static ParserOptions HasFunctionDataTransform<T>(this ParserOptions parserOptions, Delegate @delegate)
    {
        parserOptions.SetDataTransform<T>();
        parserOptions.AddMapperFunction<T>(@delegate);
        return parserOptions;
    }

    public static ParserOptions HasEncoding(this ParserOptions parserOptions, Encoding encoding)
    {
        parserOptions.FileEncoding = encoding;
        return parserOptions;
    }
}
