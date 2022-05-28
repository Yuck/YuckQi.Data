using System.Threading.Tasks;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract;

public interface IPhysicalDeletionHandler<TEntity, in TIdentifier, in TScope> where TEntity : IEntity<TIdentifier> where TIdentifier : struct
{
    TEntity Delete(TEntity entity, TScope scope);

    Task<TEntity> DeleteAsync(TEntity entity, TScope scope);
}
