using System;
using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public abstract class ActivationProviderBase<TEntity, TKey, TScope> : IActivationProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct
    {
        #region Private Members

        private readonly IRevisionProvider<TEntity, TKey, TScope> _reviser;

        #endregion


        #region Constructors

        protected ActivationProviderBase(IRevisionProvider<TEntity, TKey, TScope> reviser)
        {
            _reviser = reviser ?? throw new ArgumentNullException(nameof(reviser));
        }

        #endregion


        #region Public Methods

        public TEntity Activate(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.ActivationMomentUtc != null)
                return entity;

            entity.ActivationMomentUtc = DateTime.UtcNow;

            return _reviser.Revise(entity, scope);
        }

        public Task<TEntity> ActivateAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.ActivationMomentUtc != null)
                return Task.FromResult(entity);

            entity.ActivationMomentUtc = DateTime.UtcNow;

            return _reviser.ReviseAsync(entity, scope);
        }

        public TEntity Deactivate(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.ActivationMomentUtc == null)
                return entity;

            entity.ActivationMomentUtc = null;

            return _reviser.Revise(entity, scope);
        }

        public Task<TEntity> DeactivateAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.ActivationMomentUtc == null)
                return Task.FromResult(entity);

            entity.ActivationMomentUtc = null;

            return _reviser.ReviseAsync(entity, scope);
        }

        #endregion
    }
}