namespace ETL.Interfaces;

public interface IStrategy
{
    bool IsHandler(string type);
}
