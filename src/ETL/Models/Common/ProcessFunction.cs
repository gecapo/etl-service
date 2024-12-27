namespace ETL.Models.Common;

public sealed class ProcessFunction
{
    public FunctionType FunctionType { get; set; }
    public Delegate Function { get; set; } = null!;
}