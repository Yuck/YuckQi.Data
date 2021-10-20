using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers
{
    public class RetrievalHandler<TEntity, TKey, TScope, TRecord> : RetrievalHandlerBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle
    {
        #region Private Members

        private static readonly Type RecordType = typeof(TRecord);

        #endregion


        #region Public Methods

        protected override TEntity DoGet(TKey key, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var reader = collection.FindSync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        protected override async Task<TEntity> DoGetAsync(TKey key, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var reader = await collection.FindAsync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        protected override TEntity DoGet(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var filter = parameters.ToFilterDefinition<TRecord>();
            var reader = collection.FindSync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        protected override async Task<TEntity> DoGetAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var filter = parameters.ToFilterDefinition<TRecord>();
            var reader = await collection.FindAsync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        protected override IReadOnlyCollection<TEntity> DoGetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var filter = parameters.ToFilterDefinition<TRecord>();
            var reader = collection.FindSync(filter);
            var records = GetRecords(reader);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        protected override async Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var filter = parameters.ToFilterDefinition<TRecord>();
            var reader = await collection.FindAsync(filter);
            var records = GetRecords(reader);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        #endregion


        #region Supporting Methods

        private static IEnumerable<TRecord> GetRecords(IAsyncCursor<TRecord> reader)
        {
            var records = new List<TRecord>();

            while (reader.MoveNext())
                records.AddRange(reader.Current);

            return records;
        }

        private static TRecord GetSingleRecord(IAsyncCursor<TRecord> reader) => reader.MoveNext() ? reader.Current.SingleOrDefault() : default;

        #endregion
    }
}
