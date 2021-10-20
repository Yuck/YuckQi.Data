using MongoDB.Driver;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.DocumentDb.MongoDb.Handlers
{
    public class ActivationHandler<TEntity, TKey, TScope> : ActivationHandlerBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct where TScope : IClientSessionHandle
    {
        public ActivationHandler(IRevisionHandler<TEntity, TKey, TScope> reviser) : base(reviser) { }
    }
}
