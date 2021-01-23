using System;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class ActivationProvider<TEntity, TKey> : DataProviderBase, IActivationProvider<TEntity, TKey> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct
    {
        #region Private Members

        private readonly IRevisionProvider<TEntity, TKey> _reviser;

        #endregion


        #region Constructors

        public ActivationProvider(IUnitOfWork uow, IRevisionProvider<TEntity, TKey> reviser) : base(uow)
        {
            _reviser = reviser ?? throw new ArgumentNullException(nameof(reviser));
        }

        #endregion


        #region Public Methods

        public Task<TEntity> ActivateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.ActivationMomentUtc != null)
                return Task.FromResult(entity);

            entity.ActivationMomentUtc = DateTime.UtcNow;

            return _reviser.ReviseAsync(entity);
        }

        public Task<TEntity> DeactivateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.ActivationMomentUtc == null)
                return Task.FromResult(entity);

            entity.ActivationMomentUtc = null;

            return _reviser.ReviseAsync(entity);
        }

        #endregion
    }
}