using System.Threading.Tasks;
using YuckQi.Data.Abstract;
using YuckQi.Data.Repositories.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    internal interface IDeletionHandler<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : struct, IDeleted
    {
        Task<TEntity> DeleteAsync(TKey key, IUnitOfWork uow = null);
    }
}