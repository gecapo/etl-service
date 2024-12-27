namespace Etl.Poco.Models.Common;

public sealed record MapperResult(object Result)
{
    public List<object> Unwrap()
    {
        if (Result is IEnumerable<object> listResult)
            return [.. listResult];

        return [Result];
    }
};