using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YuckQi.Data.Sorting.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface ISearchProvider<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        Task<IPage<TEntity>> SearchAsync<TSortExpression>(IReadOnlyCollection<IDataParameter> parameters, IPage page, IOrderedEnumerable<ISortExpression<TSortExpression>> sort) where TSortExpression : class;
        Task<IPage<TEntity>> SearchAsync<TSortExpression>(object parameters, IPage page, IOrderedEnumerable<ISortExpression<TSortExpression>> sort) where TSortExpression : class;
    }
}