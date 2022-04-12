using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers
{
    public class PhysicalDeletionHandler<TEntity, TKey, TScope, TDocument> : PhysicalDeletionHandlerBase<TEntity, TKey, TScope, TDocument> where TEntity : IEntity<TKey> where TKey : struct where TScope : IDynamoDBContext
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
            var task = DoDeleteAsync(entity, scope);

            Task.WaitAll(task);

            return task.Result;
        }

        protected override async Task<Boolean> DoDeleteAsync(TEntity entity, TScope scope)
        {
            var document = Mapper.Map<TDocument>(entity);

            await scope.DeleteAsync(document);

            return true;
        }

        #endregion
    }
}
