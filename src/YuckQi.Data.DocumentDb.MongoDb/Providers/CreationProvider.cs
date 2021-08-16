﻿using System;
using System.Threading.Tasks;
using Mapster;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class CreationProvider<TEntity, TKey, TScope, TRecord> : CreationProviderBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IClientSessionHandle
    {
        #region Private Members

        private static readonly Type RecordType = typeof(TRecord);

        #endregion


        #region Protected Methods

        protected override TKey? DoCreate(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var record = entity.Adapt<TRecord>();

            collection.InsertOne(scope, record);

            return record.GetKey<TRecord, TKey>();
        }

        protected override async Task<TKey?> DoCreateAsync(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var record = entity.Adapt<TRecord>();

            await collection.InsertOneAsync(scope, record);

            return record.GetKey<TRecord, TKey>();
        }

        #endregion
    }
}
