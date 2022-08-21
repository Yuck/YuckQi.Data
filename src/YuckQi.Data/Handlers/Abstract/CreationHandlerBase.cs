using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class CreationHandlerBase<TEntity, TIdentifier, TScope> : WriteHandlerBase<TEntity>, ICreationHandler<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, ICreated where TIdentifier : struct
{
    #region Private Members

    private readonly CreationOptions<TIdentifier> _options;

    #endregion


    #region Constructors

    protected CreationHandlerBase() : this(null, null) { }

    protected CreationHandlerBase(CreationOptions<TIdentifier>? options) : this(options, null) { }

    protected CreationHandlerBase(IMapper? mapper) : this(null, mapper) { }

    protected CreationHandlerBase(CreationOptions<TIdentifier>? options, IMapper? mapper) : base(mapper)
    {
        _options = options ?? new CreationOptions<TIdentifier>();
    }

    #endregion


    #region Public Methods

    public TEntity Create(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (_options.IdentifierFactory != null)
            entity.Identifier = _options.IdentifierFactory();
        if (_options.CreationMomentAssignment == PropertyHandling.Auto)
            entity.CreationMomentUtc = DateTime.UtcNow;
        if (_options.RevisionMomentAssignment == PropertyHandling.Auto && entity is IRevised revised)
            revised.RevisionMomentUtc = entity.CreationMomentUtc;

        var identifier = DoCreate(entity, scope);
        if (identifier == null)
            throw new CreationException<TEntity>();

        entity.Identifier = identifier.Value;

        return entity;
    }

    public async Task<TEntity> Create(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (_options.IdentifierFactory != null)
            entity.Identifier = _options.IdentifierFactory();
        if (_options.CreationMomentAssignment == PropertyHandling.Auto)
            entity.CreationMomentUtc = DateTime.UtcNow;
        if (_options.RevisionMomentAssignment == PropertyHandling.Auto && entity is IRevised revised)
            revised.RevisionMomentUtc = entity.CreationMomentUtc;

        var identifier = await DoCreate(entity, scope, cancellationToken);
        if (identifier == null)
            throw new CreationException<TEntity>();

        entity.Identifier = identifier.Value;

        return entity;
    }

    #endregion


    #region Protected Methods

    protected abstract TIdentifier? DoCreate(TEntity entity, TScope scope);

    protected abstract Task<TIdentifier?> DoCreate(TEntity entity, TScope scope, CancellationToken cancellationToken);

    #endregion
}
