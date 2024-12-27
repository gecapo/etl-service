namespace Etl.Poco.Interfaces;

public interface IFactory<T> where T : IStrategy
{
    T GetStrategy(string type);
}
