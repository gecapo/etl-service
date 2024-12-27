using Etl.Poco.Models.Common;

namespace Etl.Poco.Interfaces;

public interface IParser : IStrategy
{
    Task<IList<object>> ParseData(byte[] data, ParserOptions parserOptions);
}