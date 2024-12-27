using System.Collections.Concurrent;

namespace ETL.Services;

public class PackageConcurencyService : IPackageConcurencyService
{
    private readonly ConcurrentDictionary<Type, byte> _inProccessing = [];

    public void Add(Type type) => _inProccessing.TryAdd(type, 0);
    public void Remove(Type type) => _inProccessing.TryRemove(type, out byte value);
    public bool CanRun(Type type) => _inProccessing.ContainsKey(type);
}