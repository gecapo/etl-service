using Etl.Poco.Constants;
using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;

namespace Etl.Poco.Services;

public sealed class ParserService(IFactory<IParser> parseStrategyFactory, IMapperService mapper) : IParserService
{
    private readonly IFactory<IParser> _parseStrategyFactory = parseStrategyFactory;
    private readonly IMapperService _mapper = mapper;

    public ParserType? ParseType { get; set; }
    public ParserOptions? ParserOptions { get; set; } = null!;

    public async Task<ParseResult> ParseData(int fileImportId, byte[] bytes)
    {
        _mapper.LoadConfigurationFromParser(this);
        try
        {
            var parseStrategy = _parseStrategyFactory.GetStrategy(ParseType.ToString()!);
            var parseResult = await parseStrategy.ParseData(bytes, ParserOptions);

            ParseResult result = new() { IsSuccess = true, RowsCount = 0, ParsedData = [] };
            foreach (var destinationEntityType in ParserOptions.DestinationEntityTypes!)
            {
                var records = parseResult.ToList();
                var recordArray = new List<object>();
                foreach (var mapperResult in records.Select(record =>
                             _mapper.Map(record, fileImportId, ParserOptions.SourceEntityType!, destinationEntityType)))
                {
                    recordArray.AddRange(mapperResult.Unwrap());
                }

                result.ParsedData.Add(recordArray);
                result.RowsCount += recordArray.Count;
            }

            return result;
        }
        catch (Exception ex)
        {
            return new()
                { IsSuccess = false, Message = $"{nameof(ParserService)}:{nameof(ParseResult)}: {ex.Message}" };
        }
    }
}