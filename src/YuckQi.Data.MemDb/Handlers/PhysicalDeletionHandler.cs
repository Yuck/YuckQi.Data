using System.Collections.Concurrent;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.Handlers;

public class PhysicalDeletionHandler<TEntity, TIdentifier, TScope> : PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    private readonly ConcurrentDictionary<TIdentifier, TEntity> _entities;

    public PhysicalDeletionHandler(ConcurrentDictionary<TIdentifier, TEntity> entities)
    {
        _entities = entities ?? throw new ArgumentNullException(nameof(entities));
    }

    protected override Boolean DoDelete(TEntity entity, TScope? scope)
    {
        if (entity.Identifier == null)
            throw new InvalidOperationException();

        return _entities.TryRemove(entity.Identifier, out _);
    }

    protected override Task<Boolean> DoDelete(TEntity entity, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(DoDelete(entity, scope));
}
