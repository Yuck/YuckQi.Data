using System.Data;
using YuckQi.Data.Providers.Abstract;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Sql.Dapper.Providers
{
    public class LogicalDeletionProvider<TEntity, TKey, TScope> : LogicalDeletionProviderBase<TEntity, TKey, TScope> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct where TScope : IDbTransaction
    {
        public LogicalDeletionProvider(IRevisionProvider<TEntity, TKey, TScope> reviser) : base(reviser) { }
    }
}