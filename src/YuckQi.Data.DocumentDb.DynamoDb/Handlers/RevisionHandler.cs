using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Data.Handlers.Options;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Extensions.Mapping.Abstractions;

namespace YuckQi.Data.DocumentDb.DynamoDb.Handlers
{
    public class RevisionHandler<TEntity, TKey, TScope, TDocument> : RevisionHandlerBase<TEntity, TKey, TScope, TDocument> where TEntity : IEntity<TKey>, IRevised where TKey : struct where TScope : IDynamoDBContext
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
            var task = DoReviseAsync(entity, scope);

            Task.WaitAll(task);

            return task.Result;
        }

        protected override async Task<Boolean> DoReviseAsync(TEntity entity, TScope scope)
        {
            var document = Mapper.Map<TDocument>(entity);

            await scope.SaveAsync(document); // TODO: Add cancellationToken support to all projects

            return true;
        }

        #endregion
    }
}
