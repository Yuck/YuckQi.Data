using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Write.Abstract.Interfaces;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Write.Abstract;

public abstract class RevisionHandlerBase<TEntity, TIdentifier, TScope> : RevisionHandlerBase<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier>
{
    protected RevisionHandlerBase() : this(null, null) { }

    protected RevisionHandlerBase(RevisionOptions? options) : this(options, null) { }

    protected RevisionHandlerBase(IMapper? mapper) : this(null, mapper) { }

    protected RevisionHandlerBase(RevisionOptions? options, IMapper? mapper) : base(options, mapper) { }
}

public abstract class RevisionHandlerBase<TEntity, TIdentifier, TScope, TData> : HandlerBase<TEntity, TData>, IRevisionHandler<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : IEquatable<TIdentifier>
{
    private readonly RevisionOptions _options;

    protected RevisionHandlerBase() : this(null, null) { }

    protected RevisionHandlerBase(RevisionOptions? options) : this(options, null) { }

    protected RevisionHandlerBase(IMapper? mapper) : this(null, mapper) { }

    protected RevisionHandlerBase(RevisionOptions? options, IMapper? mapper) : base(mapper)
    {
        _options = options ?? new RevisionOptions();
    }

    public TEntity Revise(TEntity entity, TScope? scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (! DoRevise(PreProcess(entity), scope))
            throw new RevisionException<TEntity, TIdentifier>(entity.Identifier);

        return entity;
    }

    public virtual IEnumerable<TEntity> Revise(IEnumerable<TEntity> entities, TScope? scope)
    {
        return entities.Select(entity => Revise(entity, scope));
    }

    public async Task<TEntity> Revise(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (! await DoRevise(PreProcess(entity), scope, cancellationToken))
            throw new RevisionException<TEntity, TIdentifier>(entity.Identifier);

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> Revise(IEnumerable<TEntity> entities, TScope? scope, CancellationToken cancellationToken)
    {
        var tasks = entities.Select(entity => Revise(entity, scope, cancellationToken));
        var results = await Task.WhenAll(tasks);

        return results;
    }

    protected abstract Boolean DoRevise(TEntity entity, TScope? scope);

    protected abstract Task<Boolean> DoRevise(TEntity entity, TScope? scope, CancellationToken cancellationToken);

    protected TEntity PreProcess(TEntity entity)
    {
        if (_options.RevisionMomentAssignment == PropertyHandling.Auto)
            entity.RevisionMomentUtc = DateTime.UtcNow;

        return entity;
    }
}
