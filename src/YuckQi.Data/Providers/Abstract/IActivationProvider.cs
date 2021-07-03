using System.Data;
using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IActivationProvider<TEntity, in TKey> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct
    {
        TEntity Activate(TEntity entity, IDbTransaction transaction);
        Task<TEntity> ActivateAsync(TEntity entity, IDbTransaction transaction);

        TEntity Deactivate(TEntity entity, IDbTransaction transaction);
        Task<TEntity> DeactivateAsync(TEntity entity, IDbTransaction transaction);
    }
}