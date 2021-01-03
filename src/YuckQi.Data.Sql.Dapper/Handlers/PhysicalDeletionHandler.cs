using System;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Abstract;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Sql.Dapper.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Handlers
{
    public class PhysicalDeletionHandler<TEntity, TKey, TRecord> : DataHandlerBase, IPhysicalDeletionHandler<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Constructors

        public PhysicalDeletionHandler(IUnitOfWork uow) : base(uow)
        {
        }

        #endregion


        #region Public Methods

        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (await Db.DeleteAsync(entity.Adapt<TRecord>(), Transaction) > 0)
                return entity;

            throw new RecordDeleteException<TRecord, TKey>(entity.Key);
        }

        #endregion
    }
}