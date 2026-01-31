using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Write.Abstract.Interfaces;
using YuckQi.Data.Handlers.Write.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Write.Abstract;

public abstract class CreationHandlerBase<TEntity, TIdentifier, TScope> : CreationHandlerBase<TEntity, TIdentifier, TScope, TEntity> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : IEquatable<TIdentifier>
{
    protected CreationHandlerBase() : this(null, null) { }

    protected CreationHandlerBase(CreationOptions<TIdentifier>? options) : this(options, null) { }

    protected CreationHandlerBase(IMapper? mapper) : this(null, mapper) { }

    protected CreationHandlerBase(CreationOptions<TIdentifier>? options, IMapper? mapper) : base(options, mapper) { }
}

public abstract class CreationHandlerBase<TEntity, TIdentifier, TScope, TData> : HandlerBase<TEntity, TData>, ICreationHandler<TEntity, TIdentifier, TScope?> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : IEquatable<TIdentifier>
{
    private readonly CreationOptions<TIdentifier> _options;

    protected CreationHandlerBase() : this(null, null) { }

    protected CreationHandlerBase(CreationOptions<TIdentifier>? options) : this(options, null) { }

    protected CreationHandlerBase(IMapper? mapper) : this(null, mapper) { }

    protected CreationHandlerBase(CreationOptions<TIdentifier>? options, IMapper? mapper) : base(mapper)
    {
        _options = options ?? new CreationOptions<TIdentifier>();
    }

    public TEntity Create(TEntity entity, TScope? scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        entity = PreProcess(entity);
        entity.Identifier = DoCreate(entity, scope) ?? throw new CreationException<TEntity>();

        return entity;
    }

    public virtual IEnumerable<TEntity> Create(IEnumerable<TEntity> entities, TScope? scope)
    {
        return entities.Select(entity => Create(entity, scope));
    }

    public async Task<TEntity> Create(TEntity entity, TScope? scope, CancellationToken cancellationToken)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        entity = PreProcess(entity);
        entity.Identifier = await DoCreate(entity, scope, cancellationToken) ?? throw new CreationException<TEntity>();

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> Create(IEnumerable<TEntity> entities, TScope? scope, CancellationToken cancellationToken)
    {
        var tasks = entities.Select(entity => Create(entity, scope, cancellationToken));
        var results = await Task.WhenAll(tasks);

        return results;
    }

    protected abstract TIdentifier? DoCreate(TEntity entity, TScope? scope);

    protected abstract Task<TIdentifier?> DoCreate(TEntity entity, TScope? scope, CancellationToken cancellationToken);

    protected TEntity PreProcess(TEntity entity)
    {
        if (_options.IdentifierFactory != null)
            entity.Identifier = _options.IdentifierFactory();
        if (_options.CreationMomentAssignment == PropertyHandling.Auto)
            entity.CreationMomentUtc = DateTime.UtcNow;
        if (_options.RevisionMomentAssignment == PropertyHandling.Auto && entity is IRevised revised)
            revised.RevisionMomentUtc = entity.CreationMomentUtc;

        return entity;
    }
}
