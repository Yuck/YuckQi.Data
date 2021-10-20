using MongoDB.Driver;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Providers
{
    public class LogicalDeletionProvider<TEntity, TKey, TScope> : LogicalDeletionHandlerBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct where TScope : IClientSessionHandle
    {
        public LogicalDeletionProvider(IRevisionHandler<TEntity, TKey, TScope> reviser) : base(reviser) { }
    }
}
