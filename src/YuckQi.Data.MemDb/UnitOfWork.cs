using System.Collections;
using YuckQi.Data.Abstract.Interfaces;

namespace YuckQi.Data.MemDb;

public class UnitOfWork<TScope> : IUnitOfWork<TScope> where TScope : new()
{
    private readonly IReadOnlyDictionary<Type, IDictionary> _entities;

    public TScope Scope { get; }

    public UnitOfWork(IReadOnlyDictionary<Type, IDictionary> entities) : this(entities, new TScope()) { }

    public UnitOfWork(IReadOnlyDictionary<Type, IDictionary> entities, TScope scope)
    {
        _entities = entities;

        Scope = scope;
    }

    public void Dispose() { }

    public IDictionary<TIdentifier, TEntity> GetEntities<TEntity, TIdentifier>()
    {
        var key = typeof(TEntity);
        var entities = _entities.TryGetValue(key, out var value)
                           ? value as IDictionary<TIdentifier, TEntity> ?? throw new InvalidCastException()
                           : throw new KeyNotFoundException($"'{key.FullName}' does not exist in the collection.");

        return entities;
    }

    public void SaveChanges() { }
}

public class UnitOfWork : UnitOfWork<Object>
{
    public UnitOfWork(IReadOnlyDictionary<Type, IDictionary> entities) : base(entities) { }

    public UnitOfWork(IReadOnlyDictionary<Type, IDictionary> entities, Object scope) : base(entities, scope) { }
}
