using System;
using System.Threading;
using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class LogicalDeletionHandlerBase<TEntity, TIdentifier, TScope> : ILogicalDeletionHandler<TEntity, TIdentifier, TScope> where TEntity : IEntity<TIdentifier>, IDeleted, IRevised where TIdentifier : struct
{
    #region Private Members

    private readonly IRevisionHandler<TEntity, TIdentifier, TScope> _reviser;

    #endregion


    #region Constructors

    protected LogicalDeletionHandlerBase(IRevisionHandler<TEntity, TIdentifier, TScope> reviser)
    {
        _reviser = reviser ?? throw new ArgumentNullException(nameof(reviser));
    }

    #endregion


    #region Public Methods

    public TEntity Delete(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (entity.DeletionMomentUtc != null)
            return entity;

        entity.DeletionMomentUtc = DateTime.UtcNow;

        return _reviser.Revise(entity, scope);
    }

    public Task<TEntity> Delete(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (entity.DeletionMomentUtc != null)
            return Task.FromResult(entity);

        entity.DeletionMomentUtc = DateTime.UtcNow;

        return _reviser.Revise(entity, scope, cancellationToken);
    }

    public TEntity Restore(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (entity.DeletionMomentUtc == null)
            return entity;

        entity.DeletionMomentUtc = null;

        return _reviser.Revise(entity, scope);
    }

    public Task<TEntity> Restore(TEntity entity, TScope scope, CancellationToken cancellationToken)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (entity.DeletionMomentUtc == null)
            return Task.FromResult(entity);

        entity.DeletionMomentUtc = null;

        return _reviser.Revise(entity, scope, cancellationToken);
    }

    #endregion
}
