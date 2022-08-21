using System.Collections.Concurrent;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.Handlers;

public class RevisionHandler<TEntity, TIdentifier, TScope> : RevisionHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct
{
    private readonly ConcurrentDictionary<TIdentifier, TEntity> _entities;

    public RevisionHandler(ConcurrentDictionary<TIdentifier, TEntity> entities, RevisionOptions? options = null) : base(options)
    {
        _entities = entities ?? throw new ArgumentNullException(nameof(entities));
    }

    protected override Boolean DoRevise(TEntity entity, TScope scope) => _entities.TryUpdate(entity.Identifier, entity, _entities.TryGetValue(entity.Identifier, out var current) ? current : throw new RevisionException<TEntity, TIdentifier>(entity.Identifier));

    protected override Task<Boolean> DoRevise(TEntity entity, TScope scope, CancellationToken cancellationToken) => Task.FromResult(DoRevise(entity, scope));
}
