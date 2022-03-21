using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using YuckQi.Data.DocumentDb.MongoDb.Extensions;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers
{
    public class PhysicalDeletionHandler<TEntity, TKey, TScope, TDocument> : PhysicalDeletionHandlerBase<TEntity, TKey, TScope, TDocument> where TEntity : IEntity<TKey> where TKey : struct where TScope : IClientSessionHandle
    {
        #region Private Members

        private static readonly Type DocumentType = typeof(TDocument);

        #endregion


        #region Constructors

        public PhysicalDeletionHandler(IMapper mapper) : base(mapper) { }

        #endregion


        #region Protected Methods

        protected override Boolean DoDelete(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
            var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
            var document = Mapper.Map<TDocument>(entity);
            var field = DocumentType.GetKeyFieldDefinition<TDocument, TKey>();
            var key = document.GetKey<TDocument, TKey>();
            var filter = Builders<TDocument>.Filter.Eq(field, key);
            var result = collection.DeleteOne(scope, filter);

            return result.DeletedCount > 0;
        }

        protected override async Task<Boolean> DoDeleteAsync(TEntity entity, TScope scope)
        {
            var database = scope.Client.GetDatabase(DocumentType.GetDatabaseName());
            var collection = database.GetCollection<TDocument>(DocumentType.GetCollectionName());
            var document = Mapper.Map<TDocument>(entity);
            var field = DocumentType.GetKeyFieldDefinition<TDocument, TKey>();
            var key = document.GetKey<TDocument, TKey>();
            var filter = Builders<TDocument>.Filter.Eq(field, key);
            var result = await collection.DeleteOneAsync(scope, filter);

            return result.DeletedCount > 0;
        }

        #endregion
    }
}
