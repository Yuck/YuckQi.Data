using System.Threading.Tasks;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IPhysicalDeletionHandler<TEntity, in TKey, in TScope> where TEntity : IEntity<TKey> where TKey : struct
{
    TEntity Delete(TEntity entity, TScope scope);

    Task<TEntity> DeleteAsync(TEntity entity, TScope scope);
}
