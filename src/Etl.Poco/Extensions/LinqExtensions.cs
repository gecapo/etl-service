namespace Etl.Poco.Extensions;

public static class LinqExtensions
{
    public static List<T> ConvertAs<T>(this IEnumerable<object> objects)
        => objects.Select(item => (T)item)
            .ToList();
}