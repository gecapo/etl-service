using CsvHelper;

namespace Etl.Poco.Models.Common;

//TODO: Create abstraction over parser options so that you could create parser specific options to not custer the base model
public sealed class ParserOptions
{
    #region CsvOptions
    public string? CsvDelimiter { get; set; } = null;
    public bool? CsvHasHeaderRecord { get; set; } = true;
    public string? CsvNewLine { get; set; } = null;
    public ShouldSkipRecord? ShouldSkipRecord { get; set; }
    #endregion

    #region BaseOptions
    /// <summary>
    /// Parser result type, e.g. MT940ParseResult
    /// </summary>
    public Type? SourceEntityType { get; private set; }

    /// <summary>
    ///  Parser Result after mapping applied
    /// </summary>
    public List<Type>? DestinationEntityTypes { get; private set; } = [];

    /// <summary>
    /// Default endogind is UTF8
    /// </summary>
    public Encoding FileEncoding { get; set; } = Encoding.UTF8;

    public Dictionary<Type, Delegate> TransformFunctionsDictionary { get; private set; } = [];
    internal void AddMapperFunction<T>(Delegate @delegate) => TransformFunctionsDictionary.Add(typeof(T), @delegate);

    internal void SetParserResult<T>() => SourceEntityType = typeof(T);
    internal void SetDataTransform<T>() => DestinationEntityTypes!.Add(typeof(T));
    #endregion
}
