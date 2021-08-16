using System;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class PhysicalDeletionProvider<TEntity, TKey, TScope, TRecord> : IPhysicalDeletionProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle
    {
        #region Private Members

        private static readonly Type RecordType = typeof(TRecord);

        #endregion


        #region Public Methods

        public TEntity Delete(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var record = entity.Adapt<TRecord>();
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var key = record.GetKey<TRecord, TKey>();
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

            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var record = entity.Adapt<TRecord>();
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var key = record.GetKey<TRecord, TKey>();
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
