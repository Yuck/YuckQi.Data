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
    public class CreationHandler<TEntity, TKey, TScope, TDocument> : CreationHandlerBase<TEntity, TKey, TScope, TDocument> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IClientSessionHandle
    {
        #region Private Members

        private static readonly Type DocumentType = typeof(TDocument);

        #endregion


        #region Constructors

        public CreationHandler(IMapper mapper) : base(mapper) { }

        public CreationHandler(IMapper mapper, CreationOptions options) : base(mapper, options) { }

        #endregion


        #region Protected Methods

        protected override TKey? DoCreate(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
            var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
            var document = Mapper.Map<TDocument>(entity);

            collection.InsertOne(scope, document);

            return document.GetKey<TDocument, TKey>();
        }

        protected override async Task<TKey?> DoCreateAsync(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
            var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
            var document = Mapper.Map<TDocument>(entity);

            await collection.InsertOneAsync(scope, document);

            return document.GetKey<TDocument, TKey>();
        }

        #endregion
    }
}
