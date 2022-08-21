using YuckQi.Data.Exceptions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope> : WriteHandlerBase<TEntity>, IPhysicalDeletionHandler<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct
{
    #region Constructors

    protected PhysicalDeletionHandlerBase() : this(null) { }

    protected PhysicalDeletionHandlerBase(IMapper? mapper) : base(mapper) { }

    #endregion


    #region Public Methods

    public TEntity Delete(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (! DoDelete(entity, scope))
            throw new PhysicalDeletionException<TEntity, TIdentifier>(entity.Identifier);

        return entity;
    }

    public async Task<TEntity> Delete(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (! await DoDelete(entity, scope, cancellationToken))
            throw new PhysicalDeletionException<TEntity, TIdentifier>(entity.Identifier);

        return entity;
    }

    #endregion


    #region Protected Methods

    protected abstract Boolean DoDelete(TEntity entity, TScope scope);

    protected abstract Task<Boolean> DoDelete(TEntity entity, TScope scope, CancellationToken cancellationToken);

    #endregion
}
