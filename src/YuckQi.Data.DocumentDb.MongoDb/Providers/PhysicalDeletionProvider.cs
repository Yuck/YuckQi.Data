using System;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Providers.Abstract;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class PhysicalDeletionProvider<TEntity, TKey, TScope, TRecord> : MongoProviderBase<TKey, TRecord>, IPhysicalDeletionProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle
    {
        #region Public Methods

        public TEntity Delete(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var field = KeyFieldDefinition;
            var record = entity.Adapt<TRecord>();
            var key = GetKey(record);
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = collection.DeleteOne(scope, filter);

            try
            {
                if (result.DeletedCount <= 0)
                    throw new RecordDeleteException<TRecord, TKey>(entity.Key);
            }
            catch (Exception exception)
            {
                throw new RecordDeleteException<TRecord, TKey>(entity.Key, exception);
            }

            return entity;
        }

        public async Task<TEntity> DeleteAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var field = KeyFieldDefinition;
            var record = entity.Adapt<TRecord>();
            var key = GetKey(record);
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = await collection.DeleteOneAsync(scope, filter);

            try
            {
                if (result.DeletedCount <= 0)
                    throw new RecordDeleteException<TRecord, TKey>(entity.Key);
            }
            catch (Exception exception)
            {
                throw new RecordDeleteException<TRecord, TKey>(entity.Key, exception);
            }

            return entity;
        }

        #endregion
    }
}