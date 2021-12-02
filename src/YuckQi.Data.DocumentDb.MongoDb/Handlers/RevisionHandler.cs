using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers
{
    public class RevisionHandler<TEntity, TKey, TScope, TRecord> : RevisionHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, IRevised where TKey : struct where TScope : IClientSessionHandle
    {
        #region Constructors

        public RevisionHandler(RevisionOptions options, IMapper mapper) : base(options, mapper) { }

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
            var record = Mapper.Map<TRecord>(entity);
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
            var record = Mapper.Map<TRecord>(entity);
            var key = record.GetKey<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = await collection.ReplaceOneAsync(scope, filter, record);

            return result.ModifiedCount > 0;
        }

        #endregion
    }
}
