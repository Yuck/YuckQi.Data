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
    public class CreationHandler<TEntity, TKey, TScope, TRecord> : CreationHandlerBase<TEntity, TKey, TScope, TRecord> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IClientSessionHandle
    {
        #region Private Members

        private static readonly Type RecordType = typeof(TRecord);

        #endregion


        #region Constructors

        public CreationHandler(IMapper mapper) : base(mapper) { }

        public CreationHandler(IMapper mapper, CreationOptions options) : base(mapper, options) { }

        #endregion


        #region Protected Methods

        protected override TKey? DoCreate(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var record = Mapper.Map<TRecord>(entity);

            collection.InsertOne(scope, record);

            return record.GetKey<TRecord, TKey>();
        }

        protected override async Task<TKey?> DoCreateAsync(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(RecordType.GetDatabaseName());
            var collection = database.GetCollection<TRecord>(RecordType.GetCollectionName());
            var record = Mapper.Map<TRecord>(entity);

            await collection.InsertOneAsync(scope, record);

            return record.GetKey<TRecord, TKey>();
        }

        #endregion
    }
}
