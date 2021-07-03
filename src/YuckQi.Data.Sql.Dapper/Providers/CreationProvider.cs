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
    public class CreationProvider<TEntity, TKey, TRecord> : ICreationProvider<TEntity, TKey> where TEntity : IEntity<TKey>, ICreated where TKey : struct
    {
        #region Public Methods

        public TEntity Create(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            entity.CreationMomentUtc = DateTime.UtcNow;

            if (! (transaction.Connection.Insert(entity.Adapt<TRecord>(), transaction) > 0))
                throw new RecordInsertException<TKey>();

            return entity;
        }

        public async Task<TEntity> CreateAsync(TEntity entity, IDbTransaction transaction)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            entity.CreationMomentUtc = DateTime.UtcNow;

            if (! (await transaction.Connection.InsertAsync(entity.Adapt<TRecord>(), transaction) > 0))
                throw new RecordInsertException<TKey>();

            return entity;
        }

        #endregion
    }
}