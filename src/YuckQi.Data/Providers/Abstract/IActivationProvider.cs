using System.Threading.Tasks;
using YuckQi.Domain.Aspects.Abstract;
using YuckQi.Domain.Entities.Abstract;

namespace YuckQi.Data.Providers.Abstract
{
    public interface IActivationProvider<TEntity, in TKey> where TEntity : IEntity<TKey>, IActivated, IRevised where TKey : struct
    {
        Task<TEntity> ActivateAsync(TEntity entity);
        Task<TEntity> DeactivateAsync(TEntity entity);
    }
}