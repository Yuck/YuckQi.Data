using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope> : CreationHandlerBase<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct
{
    private readonly ConcurrentDictionary<TIdentifier, TEntity> _entities;

    public CreationHandler(ConcurrentDictionary<TIdentifier, TEntity> entities, CreationOptions<TIdentifier> options = null) : base(options)
    {
        _entities = entities ?? throw new ArgumentNullException(nameof(entities));
    }

    protected override TIdentifier? DoCreate(TEntity entity, TScope scope) => _entities.TryAdd(entity.Identifier, entity) ? entity.Identifier : null;

    protected override Task<TIdentifier?> DoCreate(TEntity entity, TScope scope, CancellationToken cancellationToken) => Task.FromResult(DoCreate(entity, scope));
}
