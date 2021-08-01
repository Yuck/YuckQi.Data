using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IRetrievalProvider<TEntity, in TKey, in TScope, in TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TDataParameter : IDataParameter, new()
    {
        TEntity Get(TKey key, TScope scope);
        Task<TEntity> GetAsync(TKey key, TScope scope);

        TEntity Get(IReadOnlyCollection<TDataParameter> parameters, TScope scope);
        Task<TEntity> GetAsync(IReadOnlyCollection<TDataParameter> parameters, TScope scope);

        TEntity Get(Object parameters, TScope scope);
        Task<TEntity> GetAsync(Object parameters, TScope scope);

        IReadOnlyCollection<TEntity> GetList(TScope scope);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(TScope scope);

        IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<TDataParameter> parameters, TScope scope);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<TDataParameter> parameters, TScope scope);

        IReadOnlyCollection<TEntity> GetList(Object parameters, TScope scope);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, TScope scope);
    }
}