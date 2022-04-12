using System;
using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public abstract class LogicalDeletionHandlerBase<TEntity, TKey, TScope> : ILogicalDeletionHandler<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct
{
    #region Private Members

    private readonly IRevisionHandler<TEntity, TKey, TScope> _reviser;

    #endregion


    #region Constructors

    protected LogicalDeletionHandlerBase(IRevisionHandler<TEntity, TKey, TScope> reviser)
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

    public Task<TEntity> DeleteAsync(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (entity.DeletionMomentUtc != null)
            return Task.FromResult(entity);

        entity.DeletionMomentUtc = DateTime.UtcNow;

        return _reviser.ReviseAsync(entity, scope);
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

    public Task<TEntity> RestoreAsync(TEntity entity, TScope scope)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        if (scope == null)
            throw new ArgumentNullException(nameof(scope));

        if (entity.DeletionMomentUtc == null)
            return Task.FromResult(entity);

        entity.DeletionMomentUtc = null;

        return _reviser.ReviseAsync(entity, scope);
    }

    #endregion
}
