using Etl.Poco.Attributes;
using MassTransit.Internals;
using System.Reflection;

public static class FixedWidthParserExtensions
{
    public static List<FixedWidthColumnAttribute?> GetAllFixedWidthColumns(Type type) => type.GetAllProperties()
            .Where(prop => prop.GetCustomAttributes(typeof(FixedWidthColumnAttribute), false).Length > 0)
            ?.Select(prop => (FixedWidthColumnAttribute)prop?.GetCustomAttribute(typeof(FixedWidthColumnAttribute)))
            ?.ToList() ?? [];

    public static FixedWidthReportAttribute? GetFixedWidthReportAttribute(this Type type) =>
       (FixedWidthReportAttribute)type.GetCustomAttribute(typeof(FixedWidthReportAttribute))!;
}
