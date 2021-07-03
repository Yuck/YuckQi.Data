using System;
using System.Data;
using System.Threading.Tasks;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class ActivationProvider<TEntity, TKey> : IActivationProvider<TEntity, TKey> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct
    {
        #region Private Members

        private readonly IRevisionProvider<TEntity, TKey> _reviser;

        #endregion


        #region Constructors

        public ActivationProvider(IRevisionProvider<TEntity, TKey> reviser)
        {
            _reviser = reviser ?? throw new ArgumentNullException(nameof(reviser));
        }

        #endregion


        #region Public Methods

        public TEntity Activate(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (entity.ActivationMomentUtc != null)
                return entity;

            entity.ActivationMomentUtc = DateTime.UtcNow;

            return _reviser.Revise(entity, transaction);
        }

        public Task<TEntity> ActivateAsync(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (entity.ActivationMomentUtc != null)
                return Task.FromResult(entity);

            entity.ActivationMomentUtc = DateTime.UtcNow;

            return _reviser.ReviseAsync(entity, transaction);
        }

        public TEntity Deactivate(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (entity.ActivationMomentUtc == null)
                return entity;

            entity.ActivationMomentUtc = null;

            return _reviser.Revise(entity, transaction);
        }

        public Task<TEntity> DeactivateAsync(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (entity.ActivationMomentUtc == null)
                return Task.FromResult(entity);

            entity.ActivationMomentUtc = null;

            return _reviser.ReviseAsync(entity, transaction);
        }

        #endregion
    }
}