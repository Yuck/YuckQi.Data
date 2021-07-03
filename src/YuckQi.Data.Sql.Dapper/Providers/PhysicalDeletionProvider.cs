using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class PhysicalDeletionProvider<TEntity, TKey, TRecord> : IPhysicalDeletionProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        #region Public Methods

        public TEntity Delete(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction.Connection.Delete(entity.Adapt<TRecord>(), transaction) <= 0)
                throw new RecordDeleteException<TRecord, TKey>(entity.Key);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (await transaction.Connection.DeleteAsync(entity.Adapt<TRecord>(), transaction) <= 0)
                throw new RecordDeleteException<TRecord, TKey>(entity.Key);

            return entity;
        }

        #endregion
    }
}