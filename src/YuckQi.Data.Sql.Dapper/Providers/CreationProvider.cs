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
    public class CreationProvider<TEntity, TKey, TRecord, TScope> : ICreationProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IDbTransaction
    {
        #region Public Methods

        public TEntity Create(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.CreationMomentUtc == DateTime.MinValue)
                entity.CreationMomentUtc = DateTime.UtcNow;

            var key = scope.Connection.Insert<TKey?, TRecord>(entity.Adapt<TRecord>(), scope);
            if (key == null)
                throw new RecordInsertException<TRecord>();

            entity.Key = key.Value;

            return entity;
        }

        public TEntity Create<TRevised>(TRevised entity, TScope scope) where TRevised : TEntity, IRevised
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.CreationMomentUtc == DateTime.MinValue)
                entity.CreationMomentUtc = DateTime.UtcNow;
            if (entity.RevisionMomentUtc == DateTime.MinValue)
                entity.RevisionMomentUtc = entity.CreationMomentUtc;

            var key = scope.Connection.Insert<TKey?, TRecord>(entity.Adapt<TRecord>(), scope);
            if (key == null)
                throw new RecordInsertException<TRecord>();

            entity.Key = key.Value;

            return entity;
        }

        public async Task<TEntity> CreateAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.CreationMomentUtc == DateTime.MinValue)
                entity.CreationMomentUtc = DateTime.UtcNow;

            var key = await scope.Connection.InsertAsync<TKey?, TRecord>(entity.Adapt<TRecord>(), scope);
            if (key == null)
                throw new RecordInsertException<TRecord>();

            entity.Key = key.Value;

            return entity;
        }

        public async Task<TEntity> CreateAsync<TRevised>(TRevised entity, TScope scope) where TRevised : TEntity, IRevised
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.CreationMomentUtc == DateTime.MinValue)
                entity.CreationMomentUtc = DateTime.UtcNow;
            if (entity.RevisionMomentUtc == DateTime.MinValue)
                entity.RevisionMomentUtc = entity.CreationMomentUtc;

            var key = await scope.Connection.InsertAsync<TKey?, TRecord>(entity.Adapt<TRecord>(), scope);
            if (key == null)
                throw new RecordInsertException<TRecord>();

            entity.Key = key.Value;

            return entity;
        }

        #endregion
    }
}
