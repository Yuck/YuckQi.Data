using System;
using System.Threading;
using System.Threading.Tasks;
using YuckQi.Data.Exceptions;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class PhysicalDeletionHandlerBase<TEntity, TIdentifier, TScope, TRecord> : IPhysicalDeletionHandler<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct
{
    #region Properties

    protected IMapper Mapper { get; }

    #endregion


    #region Constructors

    protected PhysicalDeletionHandlerBase(IMapper mapper)
    {
        Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #endregion


    #region Public Methods

    public TEntity Delete(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (! DoDelete(entity, scope))
            throw new PhysicalDeletionException<TRecord, TIdentifier>(entity.Identifier);

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
