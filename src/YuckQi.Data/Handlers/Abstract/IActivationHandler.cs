using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Handlers.Abstract
{
    public interface IActivationHandler<TEntity, in TKey, in TScope> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct
    {
        TEntity Activate(TEntity entity, TScope scope);

        Task<TEntity> ActivateAsync(TEntity entity, TScope scope);

        TEntity Deactivate(TEntity entity, TScope scope);

        Task<TEntity> DeactivateAsync(TEntity entity, TScope scope);
    }
}
