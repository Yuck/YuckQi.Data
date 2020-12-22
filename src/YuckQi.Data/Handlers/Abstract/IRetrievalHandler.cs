using System.Collections.Generic;
using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Repositories.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    internal interface IRetrievalHandler<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        Task<TEntity> GetAsync(TKey key, IUnitOfWork uow = null);
        Task<TEntity> GetAsync(object parameters, IUnitOfWork uow = null); // TODO: Rather have this be dictionary or set of kvp?
        Task<IReadOnlyCollection<TEntity>> GetListAsync(object parameters, IUnitOfWork uow = null); // TODO: Like search, but doesn't support paging
    }
}