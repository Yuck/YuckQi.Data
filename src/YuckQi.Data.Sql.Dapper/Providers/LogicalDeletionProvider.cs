using System.Data;
using YuckQi.Data.Handlers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class LogicalDeletionProvider<TEntity, TKey, TScope> : LogicalDeletionHandlerBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct where TScope : IDbTransaction
    {
        public LogicalDeletionProvider(IRevisionHandler<TEntity, TKey, TScope> reviser) : base(reviser) { }
    }
}
