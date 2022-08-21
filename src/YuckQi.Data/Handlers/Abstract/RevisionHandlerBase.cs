using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class RevisionHandlerBase<TEntity, TIdentifier, TScope> : WriteHandlerBase<TEntity>, IRevisionHandler<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, IRevised where TIdentifier : struct
{
    #region Private Members

    private readonly RevisionOptions _options;

    #endregion


    #region Constructors

    protected RevisionHandlerBase() : this(null, null) { }

    protected RevisionHandlerBase(RevisionOptions? options) : this(options, null) { }

    protected RevisionHandlerBase(IMapper? mapper) : this(null, mapper) { }

    protected RevisionHandlerBase(RevisionOptions? options, IMapper? mapper) : base(mapper)
    {
        _options = options ?? new RevisionOptions();
    }

    #endregion


    #region Public Methods

    public TEntity Revise(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (_options.RevisionMomentAssignment == PropertyHandling.Auto)
            entity.RevisionMomentUtc = DateTime.UtcNow;

        if (! DoRevise(entity, scope))
            throw new RevisionException<TEntity, TIdentifier>(entity.Identifier);

        return entity;
    }

    public async Task<TEntity> Revise(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (_options.RevisionMomentAssignment == PropertyHandling.Auto)
            entity.RevisionMomentUtc = DateTime.UtcNow;

        if (! await DoRevise(entity, scope, cancellationToken))
            throw new RevisionException<TEntity, TIdentifier>(entity.Identifier);

        return entity;
    }

    #endregion


    #region Protected Methods

    protected abstract Boolean DoRevise(TEntity entity, TScope scope);

    protected abstract Task<Boolean> DoRevise(TEntity entity, TScope scope, CancellationToken cancellationToken);

    #endregion
}
