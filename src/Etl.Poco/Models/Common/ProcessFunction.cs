using Etl.Poco.Constants;

namespace Etl.Poco.Models.Common;

public sealed class ProcessFunction
{
    public FunctionType FunctionType { get; set; }
    public Delegate Function { get; set; } = null!;
}