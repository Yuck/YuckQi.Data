using System;
using System.Data;
using System.Threading.Tasks;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class LogicalDeletionProvider<TEntity, TKey> : ILogicalDeletionProvider<TEntity, TKey> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct
    {
        #region Private Members

        private readonly IRevisionProvider<TEntity, TKey> _reviser;

        #endregion


        #region Constructors

        public LogicalDeletionProvider(IRevisionProvider<TEntity, TKey> reviser)
        {
            _reviser = reviser ?? throw new ArgumentNullException(nameof(reviser));
        }

        #endregion


        #region Public Methods

        public TEntity Delete(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (entity.DeletionMomentUtc != null)
                return entity;

            entity.DeletionMomentUtc = DateTime.UtcNow;

            return _reviser.Revise(entity, transaction);
        }

        public Task<TEntity> DeleteAsync(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (entity.DeletionMomentUtc != null)
                return Task.FromResult(entity);

            entity.DeletionMomentUtc = DateTime.UtcNow;

            return _reviser.ReviseAsync(entity, transaction);
        }

        public TEntity Restore(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (entity.DeletionMomentUtc == null)
                return entity;

            entity.DeletionMomentUtc = null;

            return _reviser.Revise(entity, transaction);
        }

        public Task<TEntity> RestoreAsync(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (entity.DeletionMomentUtc == null)
                return Task.FromResult(entity);

            entity.DeletionMomentUtc = null;

            return _reviser.ReviseAsync(entity, transaction);
        }

        #endregion
    }
}