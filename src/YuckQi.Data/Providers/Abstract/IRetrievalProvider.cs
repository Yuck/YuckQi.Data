using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IRetrievalProvider<TEntity, in TKey, in TDataParameter> where TEntity : IEntity<TKey> where TKey : struct where TDataParameter : IDataParameter, new()
    {
        TEntity Get(TKey key, IDbTransaction transaction);
        Task<TEntity> GetAsync(TKey key, IDbTransaction transaction);

        TEntity Get(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction);
        Task<TEntity> GetAsync(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction);

        TEntity Get(Object parameters, IDbTransaction transaction);
        Task<TEntity> GetAsync(Object parameters, IDbTransaction transaction);

        IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<TDataParameter> parameters, IDbTransaction transaction);

        IReadOnlyCollection<TEntity> GetList(Object parameters, IDbTransaction transaction);
        Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, IDbTransaction transaction);
    }
}