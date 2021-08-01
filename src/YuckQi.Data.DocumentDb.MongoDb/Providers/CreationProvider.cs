using System;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Providers.Abstract;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class CreationProvider<TEntity, TKey, TScope, TRecord> : MongoProviderBase<TKey, TRecord>, ICreationProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IClientSessionHandle
    {
        #region Public Methods

        public TEntity Create(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            entity.CreationMomentUtc = DateTime.UtcNow;

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var record = entity.Adapt<TRecord>();

            collection.InsertOne(scope, record);

            var key = GetKey(record);
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

            entity.CreationMomentUtc = DateTime.UtcNow;

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var record = entity.Adapt<TRecord>();

            await collection.InsertOneAsync(scope, record);

            var key = GetKey(record);
            if (key == null)
                throw new RecordInsertException<TRecord>();

            entity.Key = key.Value;

            return entity;
        }

        #endregion
    }
}