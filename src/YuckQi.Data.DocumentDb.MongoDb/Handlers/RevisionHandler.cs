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
    public class RevisionHandler<TEntity, TKey, TScope, TDocument> : RevisionHandlerBase<TEntity, TKey, TScope, TDocument> where TEntity : IEntity<TKey>, IRevised where TKey : struct where TScope : IClientSessionHandle
    {
        #region Constructors

        public RevisionHandler(IMapper mapper) : base(mapper) { }

        public RevisionHandler(IMapper mapper, RevisionOptions options) : base(mapper, options) { }

        #endregion


        #region Private Members

        private static readonly Type DocumentType = typeof(TDocument);

        #endregion


        #region Protected Methods

        protected override Boolean DoRevise(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
            var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
            var field = DocumentType.GetKeyFieldDefinition<TDocument, TKey>();
            var document = Mapper.Map<TDocument>(entity);
            var key = document.GetKey<TDocument, TKey>();
            var filter = Builders<TDocument>.Filter.Eq(field, key);
            var result = collection.ReplaceOne(scope, filter, document);

            return result.ModifiedCount > 0;
        }

        protected override async Task<Boolean> DoReviseAsync(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
            var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
            var field = DocumentType.GetKeyFieldDefinition<TDocument, TKey>();
            var document = Mapper.Map<TDocument>(entity);
            var key = document.GetKey<TDocument, TKey>();
            var filter = Builders<TDocument>.Filter.Eq(field, key);
            var result = await collection.ReplaceOneAsync(scope, filter, document);

            return result.ModifiedCount > 0;
        }

        #endregion
    }
}
