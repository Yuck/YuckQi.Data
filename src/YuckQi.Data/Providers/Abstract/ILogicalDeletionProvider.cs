using System.Threading.Tasks;
using YuckQi.Data.Entities.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface ILogicalDeletionProvider<TEntity, in TKey> where TEntity : IEntity<TKey>, IDeleted where TKey : struct
    {
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<TEntity> RestoreAsync(TEntity entity);
    }
}