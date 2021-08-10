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
    public class RevisionProvider<TEntity, TKey, TRecord, TScope> : IRevisionProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IRevised where TKey : struct where TScope : IDbTransaction
    {
        #region Public Methods

        public TEntity Revise(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.RevisionMomentUtc == DateTime.MinValue)
                entity.RevisionMomentUtc = DateTime.UtcNow;

            if (scope.Connection.Update(entity.Adapt<TRecord>(), scope) <= 0)
                throw new RecordUpdateException<TRecord, TKey>(entity.Key);

            return entity;
        }

        public async Task<TEntity> ReviseAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.RevisionMomentUtc == DateTime.MinValue)
                entity.RevisionMomentUtc = DateTime.UtcNow;

            if (await scope.Connection.UpdateAsync(entity.Adapt<TRecord>(), scope) <= 0)
                throw new RecordUpdateException<TRecord, TKey>(entity.Key);

            return entity;
        }

        #endregion
    }
}
