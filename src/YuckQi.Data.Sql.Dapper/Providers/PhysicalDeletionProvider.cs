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
    public class PhysicalDeletionProvider<TEntity, TKey, TScope, TRecord> : IPhysicalDeletionProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDbTransaction
    {
        #region Public Methods

        public TEntity Delete(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (scope.Connection.Delete(entity.Adapt<TRecord>(), scope) <= 0)
                throw new RecordDeleteException<TRecord, TKey>(entity.Key);

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (await scope.Connection.DeleteAsync(entity.Adapt<TRecord>(), scope) <= 0)
                throw new RecordDeleteException<TRecord, TKey>(entity.Key);

            return entity;
        }

        #endregion
    }
}
