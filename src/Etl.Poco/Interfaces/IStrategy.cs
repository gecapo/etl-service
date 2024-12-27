namespace Etl.Poco.Interfaces;

public interface IStrategy
{
    bool IsHandler(string type);
}
