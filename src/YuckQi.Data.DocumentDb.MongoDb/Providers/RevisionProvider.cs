using System;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Exceptions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class RevisionProvider<TEntity, TKey, TScope, TRecord> : IRevisionProvider<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IRevised where TKey : struct where TScope : IClientSessionHandle
    {
        #region Private Members

        private static readonly Type RecordType = typeof(TRecord);

        #endregion


        #region Public Methods

        public TEntity Revise(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.RevisionMomentUtc == DateTime.MinValue)
                entity.RevisionMomentUtc = DateTime.UtcNow;

            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var record = entity.Adapt<TRecord>();
            var key = record.GetKey<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = collection.ReplaceOne(scope, filter, record);

            try
            {
                if (result.ModifiedCount <= 0)
                    throw new RecordUpdateException<TRecord, TKey>(entity.Key);
            }
            catch (Exception exception)
            {
                throw new RecordUpdateException<TRecord, TKey>(entity.Key, exception);
            }

            return entity;
        }

        public async Task<TEntity> ReviseAsync(TEntity entity, TScope scope)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (entity.RevisionMomentUtc == DateTime.MinValue)
                entity.RevisionMomentUtc = DateTime.UtcNow;

            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var field = RecordType.GetKeyFieldDefinition<TRecord, TKey>();
            var record = entity.Adapt<TRecord>();
            var key = record.GetKey<TRecord, TKey>();
            var filter = Builders<TRecord>.Filter.Eq(field, key);
            var result = await collection.ReplaceOneAsync(scope, filter, record);

            try
            {
                if (result.ModifiedCount <= 0)
                    throw new RecordUpdateException<TRecord, TKey>(entity.Key);
            }
            catch (Exception exception)
            {
                throw new RecordUpdateException<TRecord, TKey>(entity.Key, exception);
            }

            return entity;
        }

        #endregion
    }
}
