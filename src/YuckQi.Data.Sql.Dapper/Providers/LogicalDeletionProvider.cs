using System;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Entities.Abstract;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Sql.Dapper.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class LogicalDeletionProvider<TEntity, TKey> : DataProviderBase, ILogicalDeletionProvider<TEntity, TKey> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct
    {
        #region Private Members

        private readonly IRevisionProvider<TEntity, TKey> _reviser;

        #endregion


        #region Constructors

        public LogicalDeletionProvider(IUnitOfWork uow, IRevisionProvider<TEntity, TKey> reviser) : base(uow)
        {
            _reviser = reviser ?? throw new ArgumentNullException(nameof(reviser));
        }

        #endregion


        #region Public Methods

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.DeletionMomentUtc != null)
                return Task.FromResult(entity);

            entity.DeletionMomentUtc = DateTime.UtcNow;

            return _reviser.ReviseAsync(entity);
        }

        public Task<TEntity> RestoreAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.DeletionMomentUtc == null)
                return Task.FromResult(entity);

            entity.DeletionMomentUtc = null;

            return _reviser.ReviseAsync(entity);
        }

        #endregion
    }
}