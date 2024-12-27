using System.Runtime.CompilerServices;

namespace ETL.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class FixedWidthColumnAttribute(
    int start,
    int lenght,
    [CallerMemberName] string propertyName = "")
    : Attribute
{
    public int Start { get; } = start;
    public int Length { get; } = lenght;
    public string PropertyName { get; } = propertyName;
    public string? Format { get; set; }
    public bool IsOptional { get; set; } = false;
    public bool ShouldTrim { get; set; } = true;
    public int StartIndex => Start - 1;

    /// <summary>
    /// Pattern used to represent a null value.
    /// </summary>
    public string NullPattern { get; set; } = string.Empty;
}