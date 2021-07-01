using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IActivationProvider<TEntity, in TKey> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct
    {
        TEntity Activate(TEntity entity);
        Task<TEntity> ActivateAsync(TEntity entity);

        TEntity Deactivate(TEntity entity);
        Task<TEntity> DeactivateAsync(TEntity entity);
    }
}