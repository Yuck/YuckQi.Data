using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YuckQi.Data.Filtering;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IRetrievalHandler<TEntity, in TIdentifier, in TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct
{
    TEntity Get(TIdentifier identifier, TScope scope);

    Task<TEntity> GetAsync(TIdentifier identifier, TScope scope);

    TEntity Get(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

    Task<TEntity> GetAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

    TEntity Get(Object parameters, TScope scope);

    Task<TEntity> GetAsync(Object parameters, TScope scope);

    IReadOnlyCollection<TEntity> GetList(TScope scope);

    Task<IReadOnlyCollection<TEntity>> GetListAsync(TScope scope);

    IReadOnlyCollection<TEntity> GetList(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

    Task<IReadOnlyCollection<TEntity>> GetListAsync(IReadOnlyCollection<FilterCriteria> parameters, TScope scope);

    IReadOnlyCollection<TEntity> GetList(Object parameters, TScope scope);

    Task<IReadOnlyCollection<TEntity>> GetListAsync(Object parameters, TScope scope);
}
