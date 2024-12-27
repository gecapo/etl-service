namespace ETL.Interfaces;

public interface IPackageConcurencyService
{
    void Add(Type type);
    void Remove(Type type);
    bool CanRun(Type type);
}
