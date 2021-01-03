using System.Threading.Tasks;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    public interface IPhysicalDeletionHandler<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        Task<TEntity> DeleteAsync(TEntity entity);
    }
}