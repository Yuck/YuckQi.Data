using System;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Data.Providers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class RevisionProvider<TEntity, TKey, TScope, TRecord> : RevisionProviderBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, IRevised where TKey : struct where TScope : IClientSessionHandle
    {
        #region Constructors

        public RevisionProvider(RevisionOptions options) : base(options) { }

        #endregion


        #region Private Members

        private static readonly Type RecordType = typeof(TRecord);

        #endregion


        #region Protected Methods

        protected override Boolean DoRevise(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var record = entity.Adapt<TRecord>();
            var key = record.GetKey<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = collection.ReplaceOne(scope, filter, record);

            return result.ModifiedCount > 0;
        }

        protected override async Task<Boolean> DoReviseAsync(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var record = entity.Adapt<TRecord>();
            var key = record.GetKey<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = await collection.ReplaceOneAsync(scope, filter, record);

            return result.ModifiedCount > 0;
        }

        #endregion
    }
}
