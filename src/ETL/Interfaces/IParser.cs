namespace ETL.Interfaces;

public interface IParser : IStrategy
{
    Task<IList<object>> ParseData(byte[] data, ParserOptions parserOptions);
}