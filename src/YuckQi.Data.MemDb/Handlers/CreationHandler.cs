using System.Collections.Concurrent;
using CSharpFunctionalExtensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.MemDb.Handlers;

public class CreationHandler<TEntity, TIdentifier, TScope> : CreationHandlerBase<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : IEquatable<TIdentifier>
{
    private readonly ConcurrentDictionary<TIdentifier, TEntity> _entities;

    public CreationHandler(ConcurrentDictionary<TIdentifier, TEntity> entities, CreationOptions<TIdentifier>? options = null) : base(options)
    {
        _entities = entities ?? throw new ArgumentNullException(nameof(entities));
    }

    protected override Maybe<TIdentifier?> DoCreate(TEntity entity, TScope? scope)
    {
        if (entity.Identifier == null)
            throw new InvalidOperationException();

        return _entities.TryAdd(entity.Identifier, entity) ? entity.Identifier : Maybe<TIdentifier?>.None;
    }

    protected override Task<Maybe<TIdentifier?>> DoCreate(TEntity entity, TScope? scope, CancellationToken cancellationToken) => Task.FromResult(DoCreate(entity, scope));
}
