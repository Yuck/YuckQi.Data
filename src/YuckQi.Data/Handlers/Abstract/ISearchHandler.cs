using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YuckQi.Data.Sorting.Abstract;
using YuckQi.Domain.Entities.Abstract;
using YuckQi.Domain.ValueObjects.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    public interface ISearchHandler<TEntity, TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        Task<IReadOnlyCollection<TEntity>> SearchAsync(IReadOnlyCollection<IDataParameter> parameters, IPage page, IOrderedEnumerable<ISortExpression> sort);
        Task<IReadOnlyCollection<TEntity>> SearchAsync(object parameters, IPage page, IOrderedEnumerable<ISortExpression> sort);
    }
}