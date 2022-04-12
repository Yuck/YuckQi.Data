using System;
using System.Threading.Tasks;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class RevisionHandlerBase<TEntity, TKey, TScope, TRecord> : IRevisionHandler<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IRevised where TKey : struct
{
    #region Private Members

    private readonly RevisionOptions _options;

    #endregion


    #region Properties

    protected IMapper Mapper { get; }

    #endregion


    #region Constructors

    protected RevisionHandlerBase(IMapper mapper) : this(mapper, null) { }

    protected RevisionHandlerBase(IMapper mapper, RevisionOptions options)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

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
            throw new RevisionException<TRecord, TKey>(entity.Key);

        return entity;
    }

    public async Task<TEntity> ReviseAsync(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (_options.RevisionMomentAssignment == PropertyHandling.Auto)
            entity.RevisionMomentUtc = DateTime.UtcNow;

        if (! await DoReviseAsync(entity, scope))
            throw new RevisionException<TRecord, TKey>(entity.Key);

        return entity;
    }

    #endregion


    #region Protected Methods

    protected abstract Boolean DoRevise(TEntity entity, TScope scope);

    protected abstract Task<Boolean> DoReviseAsync(TEntity entity, TScope scope);

    #endregion
}
