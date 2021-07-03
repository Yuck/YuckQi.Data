using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface ILogicalDeletionProvider<TEntity, in TKey> where TEntity : IEntity<TKey>, IDeleted, IRevised where TKey : struct
    {
        TEntity Delete(TEntity entity, IDbTransaction transaction);
        Task<TEntity> DeleteAsync(TEntity entity, IDbTransaction transaction);

        TEntity Restore(TEntity entity, IDbTransaction transaction);
        Task<TEntity> RestoreAsync(TEntity entity, IDbTransaction transaction);
    }
}