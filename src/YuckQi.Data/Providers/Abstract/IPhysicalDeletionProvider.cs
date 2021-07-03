using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IPhysicalDeletionProvider<TEntity, in TKey> where TEntity : IEntity<TKey> where TKey : struct
    {
        TEntity Delete(TEntity entity, IDbTransaction transaction);
        Task<TEntity> DeleteAsync(TEntity entity, IDbTransaction transaction);
    }
}