using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Providers.Abstract;
using YuckQi.Data.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class RetrievalProvider<TEntity, TKey, TRecord, TScope, TDataParameter> : MongoProviderBase<TKey, TRecord>, IRetrievalProvider<TEntity, TKey, TScope, TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle where TDataParameter : IDataParameter, new()
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

        public TEntity Get(IReadOnlyCollection<TDataParameter> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filters = parameters.Select(t => Builders<TRecord>.Filter.Eq(new StringFieldDefinition<TRecord, Object>(t.ParameterName), t.Value));
            var filter = Builders<TRecord>.Filter.And(filters);
            var reader = collection.FindSync(filter);
            var record = GetSingleRecord(reader);
            var entity = record.Adapt<TEntity>();

            return entity;
        }

        public async Task<TEntity> GetAsync(IReadOnlyCollection<TDataParameter> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filters = parameters.Select(t => Builders<TRecord>.Filter.Eq(new StringFieldDefinition<TRecord, Object>(t.ParameterName), t.Value));
            var filter = Builders<TRecord>.Filter.And(filters);
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

            return Get(parameters.ToParameterCollection<TDataParameter>(), scope);
        }

        public Task<TEntity> GetAsync(Object parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return GetAsync(parameters.ToParameterCollection<TDataParameter>(), scope);
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

        public IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<TDataParameter> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetList(null, scope);
        }

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<TDataParameter> parameters, TScope scope)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            return DoGetListAsync(null, scope);
        }

        public IReadOnlyCollection<TEntity> GetList(Object parameters, TScope scope) => GetList(parameters?.ToParameterCollection<TDataParameter>(), scope);

        public Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, TScope scope) => GetListAsync(parameters?.ToParameterCollection<TDataParameter>(), scope);

        #endregion


        #region Supporting Methods

        private static IReadOnlyCollection<TEntity> DoGetList(IEnumerable<TDataParameter> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filters = parameters.Select(t => Builders<TRecord>.Filter.Eq(new StringFieldDefinition<TRecord, Object>(t.ParameterName), t.Value));
            var filter = Builders<TRecord>.Filter.And(filters);
            var reader = collection.FindSync(filter);
            var records = GetRecords(reader);
            var entities = records.Adapt<IReadOnlyCollection<TEntity>>();

            return entities;
        }

        private static async Task<IReadOnlyCollection<TEntity>> DoGetListAsync(IEnumerable<TDataParameter> parameters, TScope scope)
        {
            var database = scope.Client.GetDatabase(DatabaseName);
            var collection = database.GetCollection<TRecord>(CollectionName);
            var filters = parameters.Select(t => Builders<TRecord>.Filter.Eq(new StringFieldDefinition<TRecord, Object>(t.ParameterName), t.Value));
            var filter = Builders<TRecord>.Filter.And(filters);
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
