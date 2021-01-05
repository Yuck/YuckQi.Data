using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IRetrievalProvider<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        Task<TEntity> GetAsync(TKey key);
        Task<TEntity> GetAsync(IReadOnlyCollection<IDataParameter> parameters);
        Task<TEntity> GetAsync(object parameters);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<IDataParameter> parameters = null);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(object parameters = null);
    }
}