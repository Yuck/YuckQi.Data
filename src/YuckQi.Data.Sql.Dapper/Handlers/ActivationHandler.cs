using System;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Entities.Abstract;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Handlers
{
    public class ActivationHandler<TEntity, TKey> : DataHandlerBase, IActivationHandler<TEntity, TKey> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct
    {
        #region Private Members

        private readonly IRevisionHandler<TEntity, TKey> _reviser;

        #endregion


        #region Constructors

        public ActivationHandler(IUnitOfWork uow, IRevisionHandler<TEntity, TKey> reviser) : base(uow)
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