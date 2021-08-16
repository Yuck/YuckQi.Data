using System;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class PhysicalDeletionProvider<TEntity, TKey, TScope, TRecord> : PhysicalDeletionProviderBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle
    {
        #region Private Members

        private static readonly Type RecordType = typeof(TRecord);

        #endregion


        #region Protected Methods

        protected override Boolean DoDelete(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var record = entity.Adapt<TRecord>();
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var key = record.GetKey<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = collection.DeleteOne(scope, filter);

            return result.DeletedCount > 0;
        }

        protected override async Task<Boolean> DoDeleteAsync(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var record = entity.Adapt<TRecord>();
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var key = record.GetKey<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = await collection.DeleteOneAsync(scope, filter);

            return result.DeletedCount > 0;
        }

        #endregion
    }
}
