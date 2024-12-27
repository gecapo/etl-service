namespace ETL.Interfaces;

public interface IFactory<T> where T : IStrategy
{
    T GetStrategy(string type);
}
