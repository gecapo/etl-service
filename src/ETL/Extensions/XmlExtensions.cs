using System.Xml.Serialization;

namespace ETL.Extensions;

public static class XmlExtensions
{
    public static TResult Deserialize<TResult>(string xml) where TResult : class
    {
        using var sr = new StringReader(xml);
        var xs = new XmlSerializer(typeof(TResult));

        return (TResult)xs.Deserialize(sr);
    }
}