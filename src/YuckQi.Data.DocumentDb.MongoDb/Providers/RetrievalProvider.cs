using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.DocumentDb.MongoDb.Providers.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Filtering;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class RetrievalProvider<TEntity, TKey, TRecord, TScope> : MongoProviderBase<TKey, TRecord>, IRetrievalProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle
    {
        #region Public Methods

        public TEntity Get(TKey key, TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = Builders<TRecord>.Filter.Eq(KeyFieldDefinition, key);
            var reader = collection.FindSync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(TKey key, TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = Builders<TRecord>.Filter.Eq(KeyFieldDefinition, key);
            var reader = await collection.FindAsync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public TEntity Get(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = parameters.ToFilterDefinition<TRecord>();
            var reader = collection.FindSync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = parameters.ToFilterDefinition<TRecord>();
            var reader = await collection.FindAsync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public TEntity Get(Object parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return Get(parameters.ToFilterCollection(), scope);
        }

        public Task<TEntity> GetAsync(Object parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return GetAsync(parameters.ToFilterCollection(), scope);
        }

        public IReadOnlyCollection<TEntity> GetList(TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetList(null, scope);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(TScope scope)
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetListAsync(null, scope);
        }

        public IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetList(null, scope);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetListAsync(null, scope);
        }

        public IReadOnlyCollection<TEntity> GetList(Object parameters, TScope scope) => GetList(parameters?.ToFilterCollection(), scope);

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, TScope scope) => GetListAsync(parameters?.ToFilterCollection(), scope);

        #endregion


        #region Supporting Methods

        private static IReadOnlyCollection<TEntity> DoGetList(IEnumerable<FilterCriteria> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = parameters.ToFilterDefinition<TRecord>();
            var reader = collection.FindSync(filter);
            var records = GetRecords(reader);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        private static async Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IEnumerable<FilterCriteria> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filter = parameters.ToFilterDefinition<TRecord>();
            var reader = await collection.FindAsync(filter);
            var records = GetRecords(reader);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

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
