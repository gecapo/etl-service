using System.Xml.Serialization;

namespace ETL.Strategies.Parsers
{
    public class XmlParseStrategy : IParser, IStrategy
    {
        public bool IsHandler(string type) => type == ParserType.Xml.ToString();

        public async Task<IList<object>> ParseData(byte[] data, ParserOptions parserOptions)
        {
            var encoding = parserOptions.FileEncoding;

            using var memomryStream = new MemoryStream(data);
            using var streamReader = new StreamReader(memomryStream, encoding);
            XmlSerializer xmlSerializer = new(parserOptions.SourceEntityType!);

            var parsed = xmlSerializer.Deserialize(streamReader);
            List<object> result = [];
            if (parsed != null)
                result.Add(parsed);

            return await Task.FromResult(result);
        }
    }
}