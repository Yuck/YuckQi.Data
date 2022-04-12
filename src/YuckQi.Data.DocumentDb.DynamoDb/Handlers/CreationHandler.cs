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
    public class CreationHandler<TEntity, TKey, TScope, TDocument> : CreationHandlerBase<TEntity, TKey, TScope, TDocument> where TEntity : IEntity<TKey>, ICreated where TKey : struct where TScope : IDynamoDBContext
    {
        #region Constructors

        public CreationHandler(IMapper mapper) : base(mapper) { }

        public CreationHandler(IMapper mapper, CreationOptions options) : base(mapper, options) { }

        #endregion


        #region Protected Methods

        protected override TKey? DoCreate(TEntity entity, TScope scope) => throw new NotImplementedException();

        protected override async Task<TKey?> DoCreateAsync(TEntity entity, TScope scope)
        {
            var document = Mapper.Map<TDocument>(entity);

            await scope.SaveAsync(document); // TODO: Add cancellationToken support to all projects

            return entity.Key;
        }

        #endregion
    }
}
