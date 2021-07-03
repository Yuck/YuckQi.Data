using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mapster;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class RevisionProvider<TEntity, TKey, TRecord> : IRevisionProvider<TEntity, TKey> where TEntity : IEntity<TKey>, IRevised where TKey : struct
    {
        #region Public Methods

        public TEntity Revise(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            entity.RevisionMomentUtc = DateTime.UtcNow;

            if (transaction.Connection.Update(entity.Adapt<TRecord>(), transaction) <= 0)
                throw new RecordUpdateException<TRecord, TKey>(entity.Key);

            return entity;
        }

        public async Task<TEntity> ReviseAsync(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            entity.RevisionMomentUtc = DateTime.UtcNow;

            if (await transaction.Connection.UpdateAsync(entity.Adapt<TRecord>(), transaction) <= 0)
                throw new RecordUpdateException<TRecord, TKey>(entity.Key);

            return entity;
        }

        #endregion
    }
}