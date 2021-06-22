using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IRetrievalProvider<TEntity, in TKey, in TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TDataParameter : IDataParameter, new()
    {
        Task<TEntity> GetAsync(TKey key);
        Task<TEntity> GetAsync(IReadOnlyCollection<TDataParameter> parameters);
        Task<TEntity> GetAsync(Object parameters);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<TDataParameter> parameters = null);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters = null);
    }
}