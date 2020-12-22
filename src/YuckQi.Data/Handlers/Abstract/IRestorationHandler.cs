using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Repositories.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    internal interface IRestorationHandler<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : struct, IDeleted
    {
        Task<TEntity> RestoreAsync(TKey key, IUnitOfWork uow = null);
    }
}