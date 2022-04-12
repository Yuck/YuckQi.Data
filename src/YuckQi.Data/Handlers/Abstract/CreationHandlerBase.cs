using System;
using System.Threading.Tasks;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class CreationHandlerBase<TEntity, TKey, TScope, TRecord> : ICreationHandler<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, ICreated where TKey : struct
{
    #region Private Members

    private readonly CreationOptions _options;

    #endregion


    #region Properties

    protected IMapper Mapper { get; }

    #endregion


    #region Constructors

    protected CreationHandlerBase(IMapper mapper) : this(mapper, null) { }

    protected CreationHandlerBase(IMapper mapper, CreationOptions options)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _options = options ?? new CreationOptions();
    }

    #endregion


    #region Public Methods

    public TEntity Create(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (_options.CreationMomentAssignment == PropertyHandling.Auto)
            entity.CreationMomentUtc = DateTime.UtcNow;
        if (_options.RevisionMomentAssignment == PropertyHandling.Auto && entity is IRevised revised)
            revised.RevisionMomentUtc = entity.CreationMomentUtc;

        var key = DoCreate(entity, scope);
        if (key == null)
            throw new CreationException<TRecord>();

        entity.Key = key.Value;

        return entity;
    }

    public async Task<TEntity> CreateAsync(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (_options.CreationMomentAssignment == PropertyHandling.Auto)
            entity.CreationMomentUtc = DateTime.UtcNow;
        if (_options.RevisionMomentAssignment == PropertyHandling.Auto && entity is IRevised revised)
            revised.RevisionMomentUtc = entity.CreationMomentUtc;

        var key = await DoCreateAsync(entity, scope);
        if (key == null)
            throw new CreationException<TRecord>();

        entity.Key = key.Value;

        return entity;
    }

    #endregion


    #region Protected Methods

    protected abstract TKey? DoCreate(TEntity entity, TScope scope);

    protected abstract Task<TKey?> DoCreateAsync(TEntity entity, TScope scope);

    #endregion
}
